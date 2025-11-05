using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using CsvHelper;
using System.Globalization;
using Path = System.IO.Path;

namespace ConsoleApplication
{
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
                    Program.LogWithRed("Failed to parse FilterValue as DateTime.");
                }
                else
                    Program.LogWithRed("FilterValue node not found in XML.");
            }
            catch (Exception ex)
            {
                Program.LogWithRed("Error extracting FilterValue: " + ex.Message);
            }
            return DateTime.MinValue;
        }
    }

    class Program
    {
        private const int STD_OUTPUT_HANDLE = -11;
        private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);


        public static DateTime currentDate = DateTime.Now;
        public static string logFolderPath = "Logs";
        public static string exceptionReportFolderPath = "ExceptionReport";
        public static int exitcode = -1;
        private static int passCount = 0;
        private static int methodCount = 0;
        private static int methodCount1 = 0;
        public static string logFilePath = Path.Combine(logFolderPath, $"FileModification_Log_{currentDate.ToString("yyyyMMdd")}.txt");
        private static string logFilename;
        static string inputFolder = "";
        public static string outputFolder = "";
        static string functionName = "";
        static string individualFile = "";
        static string fileToFunction = "";
        static string havetoappend = "";
        static string subfoldername = "";
        static string createsubfolder = "";
        static string callscript = "";
        static string scriptname = "";
        static string changeextensionto = "";
        static string changename = "";
        static string renameconfig = "";
        static string newfilename = "";
        static string args = "";
        static string SplitParameter = "";
        public static DateTime processDate = GetProcessDate(Directory.GetCurrentDirectory() + @"\Config\ProcessDateXmlPath.txt");

        static Dictionary<string, string> nameconfig = new Dictionary<string, string>();

        //public static void LogWithRed(string message)
        //{
        //    Console.ForegroundColor = ConsoleColor.Red;  // Set color to red
        //    Console.WriteLine(message);
        //    Console.ForegroundColor = ConsoleColor.Green;  // Reset back to green// Set the console text color based on the input color argument
        //}

        private static void EnableAnsiSupport()
        {
            var handle = GetStdHandle(STD_OUTPUT_HANDLE);
            if (GetConsoleMode(handle, out var mode))
            {
                mode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;
                SetConsoleMode(handle, mode);
            }
        }

        public static void LogWithRed(string message)
        {
            EnableAnsiSupport();
            const string redColor = "\u001b[31m";  // ANSI escape code for red text
            const string resetColor = "\u001b[0m"; // ANSI escape code to reset color

            Console.WriteLine($"{redColor}{message}{resetColor}");
        }

        static void Main(string[] args)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                //InitializeLogFile();
                //string folderPath = File.ReadLines(@"configpath.txt").FirstOrDefault();
                string folderPath = Directory.GetCurrentDirectory() + @"\Input_Files";
                string mainfilePath = Directory.GetCurrentDirectory() + @"\config\MainFile.csv";

                // string configFile = "path/to/your/config.xlsx";

                if (File.Exists(mainfilePath))
                {
                    ProcessConfiguration(mainfilePath);
                }
                else
                {
                    exitcode = 1;
                    LogWithRed("Config file not found: " + mainfilePath);
                    CreateLogs("Config file not found: " + mainfilePath, logFilePath);
                }

                if (methodCount1 == passCount)
                {
                    CreateLogs("File modifications completed successfully", logFilePath);
                    Console.WriteLine("File modifications completed successfully");
                    exitcode = 0;
                }
                else if (methodCount1 < passCount)
                {
                    CreateLogs("File modifications completed partially", logFilePath);
                    Console.WriteLine("File modifications completed partially");
                }
                else if (methodCount1 > passCount)
                {
                    CreateLogs("Method count incremented unnecessary", logFilePath);
                    Console.WriteLine("Method count incremented unnecessary");
                }

                Console.WriteLine("Methods used in conversion:" + methodCount1);
                CreateLogs("Methods used in conversion", logFilePath);
            }
            catch (Exception ex)
            {
                LogWithRed(ex.Message.ToString());
                CreateLogs(ex.Message.ToString(), logFilePath);
            }
            finally
            {
                Environment.Exit(exitcode);
            }
        }

        public static Excel.Range FindCellWithValue(Excel.Worksheet worksheet, string value)
        {
            foreach (Excel.Range cell in worksheet.UsedRange.Cells)
            {
                if (cell.Value != null && cell.Value.ToString() == value)
                    return cell;
            }
            return null; // Cell not found
        }
        //public static void changefile(string filePath) {
        //    string logmessage = "";
        //    Excel.Workbook workbook = null;
        //    Excel.Application excelApp = null;
        //    Excel.Worksheet worksheet = null;
        //    string fname = Path.GetFileName(filePath);
        //    if (newfilename != "")
        //    {
        //        fname = newfilename;
        //    }

        //    string csvFilePath = outputFolder + @"\" + fname;
        //    try
        //    {

        //        excelApp = new Excel.Application();

        //        excelApp.DisplayAlerts = false; // Disable alerts to avoid any pop-ups during processing

        //        workbook = excelApp.Workbooks.Open(filePath);

        //        // Iterate through all sheets
        //        foreach (Excel.Worksheet sheet in workbook.Sheets)
        //        {
        //            // Iterate through all rows and cells
        //            foreach (Excel.Range row in sheet.UsedRange.Rows)
        //            {
        //                // Skip the first row
        //                if (row.Row == 1)
        //                    continue;

        //                // Copy data from cell 1 to cell 3
        //                Excel.Range cell1 = (Excel.Range)row.Cells[1];
        //                Excel.Range cell3 = (Excel.Range)row.Cells[3];
        //                cell3.Value = @"D:\Temp Download#"+cell1.Value;

        //                // Clear formatting of cell 3 (optional)
        //               // cell3.ClearFormats();

        //               // Marshal.ReleaseComObject(cell1);
        //                //Marshal.ReleaseComObject(cell3);
        //            }

        //            //System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
        //        }

        //        // Save the changes back to the file


        //        // workbook.Save();
        //        workbook.SaveAs(csvFilePath, Excel.XlFileFormat.xlWorkbookDefault);
        //        workbook.Close(false);
        //        excelApp.Quit();

        //        logmessage += Environment.NewLine + $"Cell styles cleared successfully in file: {filePath}";
        //    }
        //    catch (Exception ex)
        //    {
        //        logmessage += Environment.NewLine + $"Error clearing format of sheet in file '{filePath}': {ex.Message}";
        //    }
        //    finally
        //    {
        //        // Close and release resources
        //        //workbook.Close(false);
        //        //excelApp.Quit();

        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
        //    }

        //    Console.WriteLine(logmessage);

        //}
        public static void JPMSwap(string filePath)
        {
            string password = "NexusLY1";
            string modifiedFileFolder = "";
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = null;
            excelApp.DisplayAlerts = false;

            try
            {
                // Disable alerts to prevent any pop-up messages

                workbook = excelApp.Workbooks.Open(filePath, Password: password);
            }
            catch (Exception ex)
            {
                exitcode = 4;
                LogWithRed(ex.Message.ToString());
                CreateLogs(ex.Message, logFilePath);
                return;
            }

            try
            {
                Excel.Worksheet Component_Underlyings = null;
                foreach (Excel.Worksheet sheet in workbook.Sheets)
                {
                    if (sheet.Name == "Component Underlyings")
                    {
                        Component_Underlyings = sheet;
                    }
                    else
                    {
                        if (sheet.Name == "Overview")
                        {
                            if (sheet != null)
                            {
                                // Find the cell with the value "Strategy Name"
                                Excel.Range strategyNameCell = FindCellWithValue(sheet, "Strategy Name");

                                if (strategyNameCell != null)
                                {
                                    // Get the column index of the "Strategy Name" cell
                                    int columnIndex = strategyNameCell.Column;

                                    // Iterate through the specified column ("cx")
                                    Excel.Range columnCx = sheet.Columns[columnIndex];

                                    foreach (Excel.Range cell in columnCx.Cells)
                                    {
                                        if (cell.Value == "Synthetic Cash Position (SCA)")
                                        {
                                            // Get the row index of the "Synthetic Cash Position (SCA)" cell
                                            int rowIndex = cell.Row;
                                            int colIndex = cell.Column;
                                            int nonEmptyCell = 0;
                                            for (int col = colIndex; col <= sheet.Columns.Count; col++)
                                            {
                                                Excel.Range cell1 = (Excel.Range)sheet.Cells[rowIndex, col];
                                                if (cell1.Value == null || string.IsNullOrEmpty(cell1.Value.ToString()))
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    nonEmptyCell++;
                                                }
                                            }


                                            // Iterate through the found row to get the value of the last cell
                                            Excel.Range lastCell = sheet.Cells[rowIndex, colIndex + nonEmptyCell - 1];

                                            // Print the value of the last cell
                                            Console.WriteLine("Value of last cell in the row: " + lastCell.Value, "green");
                                            CreateLogs("Value of last cell in the row: " + lastCell.Value, logFilePath);

                                            break; // Exit the loop once the value is found
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Cell with value 'Strategy Name' not found.");
                                    CreateLogs("Cell with value 'Strategy Name' not found.", logFilePath);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Worksheet not found.");
                                CreateLogs("Worksheet not found.", logFilePath);
                            }
                        }
                        sheet.Delete();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);

                    }
                }
                //clear format 
                foreach (Excel.Range row in Component_Underlyings.UsedRange.Rows)
                {
                    foreach (Excel.Range cell in row.Cells)
                    {
                        // Clear cell style
                        //cell.Style = null;
                        cell.ClearFormats();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(cell);
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(row);

                }
                // Pasting "A" on (1,1) cell
                CopyAndPaste(Component_Underlyings, 1, 1, 1, 1, 1, 1, "A");
                try
                {
                    string currentDirectory = Path.GetDirectoryName(filePath);
                    string fname = Path.GetFileNameWithoutExtension(filePath);
                    if (newfilename != "")
                    {
                        fname = newfilename;
                    }
                    string csvFilePath = outputFolder + "\\" + fname;


                    // Save the worksheet as CSV
                    Component_Underlyings.SaveAs(csvFilePath, Excel.XlFileFormat.xlCSV);

                    Console.WriteLine("Conversion to CSV completed for " + fname);
                    CreateLogs("Conversion to CSV completed for " + fname, logFilePath);
                }
                catch (Exception ex)
                {
                    exitcode = 3;
                    LogWithRed("Error: " + ex.Message);
                    CreateLogs("Error: " + ex.Message, logFilePath);
                }
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close(false);
                excelApp.Quit();
            }
        }
        static void ProcessConfiguration(string configFile)
        {
            try
            {
                System.Data.DataTable mainFileData = DataFromCsvFile(configFile);
                passCount = mainFileData.Rows.Count;
                foreach (DataRow row in mainFileData.Rows)
                {

                    // Accessing values in each column
                    foreach (DataColumn col in mainFileData.Columns)
                    {
                        newfilename = "";
                        // Console.Write(col.ToString());

                        //Console.WriteLine(row[col]);
                        if (col.ToString().Trim() == "Input")
                        {
                            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

                            inputFolder = row[col].ToString();
                            subfoldername = Path.GetFileName(inputFolder);
                            inputFolder = currentDirectory + $"{inputFolder}\\";
                        }
                        else if (col.ToString().Trim() == "Output")
                        {
                            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                            outputFolder = row[col].ToString();
                            outputFolder = currentDirectory + $"{outputFolder}\\";
                        }
                        else if (col.ToString().Trim() == "FunctionName")
                        {
                            functionName = row[col].ToString();
                        }
                        else if (col.ColumnName == "Individual_File")
                        {
                            individualFile = row[col].ToString();
                        }
                        else if (col.ToString().Trim() == "File_Function")
                        {
                            fileToFunction = row[col].ToString();
                        }
                        else if (col.ToString().Trim() == "havetoappend")
                        {
                            havetoappend = row[col].ToString();
                        }
                        else if (col.ToString().Trim() == "createsubfolder")
                        {
                            createsubfolder = row[col].ToString();
                        }
                        else if (col.ToString().Trim() == "callscript")
                        {
                            callscript = row[col].ToString();
                        }
                        else if (col.ToString().Trim() == "scriptname")
                        {
                            scriptname = row[col].ToString();

                        }
                        else if (col.ToString().Trim() == "extension")
                        {
                            changeextensionto = row[col].ToString();

                        }
                        else if (col.ToString().Trim() == "rename")
                        {
                            changename = row[col].ToString();

                        }
                        else if (col.ToString().Trim() == "renameconfig")
                        {

                            renameconfig = row[col].ToString();
                            renameconfig = ReplaceDateTimePlaceholders(renameconfig);
                        }
                        else if (col.ToString().Trim() == "arguments")
                        {
                            args = row[col].ToString();
                            args = ReplaceDateTimePlaceholders(args);
                        }
                        else if (col.ToString().Trim() == "SplitParameter")
                        {
                            SplitParameter = row[col].ToString();
                        }


                    }
                    if (changename.ToLower().Trim() == "true")
                    {
                        nameconfig = ReadConfignames(renameconfig);
                    }
                    if (createsubfolder.ToLower().Trim() == "true")
                    {
                        string outputsubfolder = Path.GetFileName(outputFolder);
                        if (subfoldername.ToLower().Trim() != outputsubfolder.ToLower().Trim())
                        {
                            outputFolder += (@"\" + subfoldername);
                        }
                    }
                    if (outputFolder != "")
                    {
                        if (!Directory.Exists(outputFolder))
                        {
                            // If it doesn't exist, create the folder
                            Directory.CreateDirectory(outputFolder);
                            Console.WriteLine(" Output Folder created successfully : " + outputFolder);
                        }
                    }

                    if (callscript.ToLower().Trim() == "true")
                    {
                        string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

                        string scriptFolderPath = currentDirectory + @"Config\";
                        string sourceFolderPath = inputFolder;
                        string destinationFolderPath = outputFolder;

                        string scriptFileName = "append.bat";

                        string fileName = "newfile.txt";

                        string scriptFilePath = Path.Combine(scriptFolderPath, scriptFileName);
                        string scriptrunpath = Path.Combine(inputFolder, scriptFileName);

                        string sourceFilePath = Path.Combine(sourceFolderPath, fileName);

                        string destinationFilePath = Path.Combine(destinationFolderPath, fileName);

                        MoveScriptFile(scriptFilePath, sourceFolderPath, scriptFileName);


                        RunScript(scriptrunpath);

                        MoveFileToNewFolder(sourceFilePath, destinationFilePath);

                    }
                    if (individualFile.ToLower().Trim() == "true")
                    {
                        //Console.WriteLine(Path.GetFileName(inputFolder));
                        // Process individual file using fileToFunction
                        ProcessIndividualFile(inputFolder, outputFolder, fileToFunction);

                    }
                    else
                    {
                        // Process all files in inputFolder using functionName
                        ProcessAllFiles(inputFolder, outputFolder, functionName);
                    }

                    //  Console.WriteLine(); // Move to the next line for the next row
                }
            }
            catch (Exception ex)
            {
                exitcode = 5;
                LogWithRed("Error extracting files: " + ex.Message);
                CreateLogs("Error extracting files: " + ex.Message, logFilePath);
            }
        }

        static void UnzipFiles(string zipFilePath)
        {
            string extractFolderPath = outputFolder;
            try
            {
                // Create the extraction folder if it doesn't exist
                if (!Directory.Exists(extractFolderPath))
                {
                    Directory.CreateDirectory(extractFolderPath);
                }

                // Extract the files from the zip archive
                System.IO.Compression.ZipFile.ExtractToDirectory(zipFilePath, extractFolderPath);

                Console.WriteLine("Files successfully extracted to: " + extractFolderPath);
                CreateLogs("Files successfully extracted to: " + extractFolderPath, logFilePath);
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error extracting files: " + ex.Message);
                CreateLogs("Error extracting files: " + ex.Message, logFilePath);
            }
        }
        public static void DeleteRow(Excel.Worksheet worksheet, int rowNumber)
        {
            try
            {
                Excel.Range rowToDelete = worksheet.Rows[rowNumber];

                Console.WriteLine("Cell Value: " + worksheet.Cells[rowNumber, 1].Value);
                rowToDelete.Delete();
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error extracting files: " + ex.Message);
                CreateLogs("Error extracting files: " + ex.Message, logFilePath);
            }
        }

        public static void ChangeColumnsToDate(string filePath)
        {
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string[] arr = args.Split('#');
            List<string> columnNames = new List<string>(arr);
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet worksheet = workbook.ActiveSheet;
            excelApp.DisplayAlerts = false;

            Excel.Range usedRange = worksheet.UsedRange;

            try
            {
                foreach (string columnName in columnNames)
                {
                    Excel.Range targetColumn = null;

                    // Find the target column by searching for the column name in each row
                    for (int rowIndex = 1; rowIndex <= usedRange.Rows.Count; rowIndex++)
                    {
                        for (int columnIndex = 1; columnIndex <= usedRange.Columns.Count; columnIndex++)
                        {
                            Excel.Range cell = (Excel.Range)usedRange.Cells[rowIndex, columnIndex];
                            if (cell.Value2 != null && cell.Value2.ToString() == columnName)
                            {
                                targetColumn = worksheet.Range[cell, worksheet.Cells[worksheet.Rows.Count, cell.Column]];
                                break;
                            }
                            // Your logic here
                        }
                    }

                    if (targetColumn != null)
                    {
                        // Change the format of the target column to mm/dd/yyyy
                        targetColumn.NumberFormat = "MM/dd/yyyy";
                    }
                    else
                    {
                        Console.WriteLine($"Column '{columnName}' not found.");
                        CreateLogs($"Column '{columnName}' not found.", logFilePath);
                    }
                }

                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();
            }
        }

        public static void MoveSheetToFirst(string filePath)
        {
            string modifiedFileFolder = outputFolder;
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            excelApp.DisplayAlerts = false;
            string[] arr = args.Split('#');
            try
            {

                Excel.Worksheet sheetToMove = workbook.Sheets[arr[0]];

                // Move the worksheet to the first position
                sheetToMove.Move(workbook.Sheets[1]);
                string modified_filePath = Path.Combine(modifiedFileFolder, Path.GetFileName(filePath));
                workbook.SaveAs(modified_filePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }

        }
        public static void Pershing(string filePath)
        {
            string modifiedFileFolder = outputFolder;
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            excelApp.DisplayAlerts = false;

            try
            {
                // Replace 5 with the desired size of your array
                Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1];

                int rowCount = worksheet.UsedRange.Rows.Count;
                int startRow = 2;//First Equity is written to be copied...
                int columnIndex = 7;
                int[] rowsToDelete = { 2, 3, 4, 6, rowCount };
                Array.Sort(rowsToDelete, (a, b) => b.CompareTo(a));
                //for (int sheetIndex = 0; sheetIndex < workbook.Sheets.Count; sheetIndex++)
                //{
                foreach (int r in rowsToDelete)
                {
                    int row = r;
                    DeleteRow(worksheet, row);
                }
                rowCount = worksheet.UsedRange.Rows.Count;
                int blankCellCount = 0;
                for (int row = startRow; row <= rowCount; row++)
                {
                    Excel.Range cell = (Excel.Range)worksheet.Cells[row, columnIndex];
                    if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                    {
                        blankCellCount++;
                    }
                    else
                    {
                        break;
                    }
                }
                string valueToCopied = worksheet.Name.ToString();
                CopyAndPaste(worksheet, startRow - 1, columnIndex, startRow, columnIndex, startRow + blankCellCount - 1, columnIndex, valueToCopied);
                string modified_filePath = Path.Combine(modifiedFileFolder, Path.GetFileName(filePath));
                workbook.SaveAs(modified_filePath);
                //clear format

                //}


            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();


                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }
        public static void Comerica(string filePath)
        {
            string modifiedFileFolder = outputFolder;
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            excelApp.DisplayAlerts = false;

            try
            {

                int startRow = GetRowIndexOfFirstOccurrence(filePath, 1, "EQUITIES"); //First Equity is written to be copied...
                int columnIndex = 1;
                //for (int sheetIndex = 0; sheetIndex < workbook.Sheets.Count; sheetIndex++)
                //{
                Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1];
                int rowCount = worksheet.UsedRange.Rows.Count;
                int blankCellCount = 0;
                CopyAndPaste(worksheet, 2, 1, 3, 1, startRow - 1, 1, null);
                for (int row = startRow + 1; row <= rowCount; row++)
                {
                    Excel.Range cell = (Excel.Range)worksheet.Cells[row, columnIndex];
                    if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                    {
                        blankCellCount++;
                    }
                    else
                    {
                        break;
                    }
                }
                CopyAndPaste(worksheet, startRow, columnIndex, startRow, columnIndex, startRow + blankCellCount, columnIndex, null);
                string modified_filePath = Path.Combine(modifiedFileFolder, Path.GetFileName(filePath));
                ClearFormatworkbook(modified_filePath, workbook);

                workbook.SaveAs(modified_filePath, Excel.XlFileFormat.xlCSV);
                //clear format

                //}


            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();


                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }
        static void CopyAndPaste(Excel.Worksheet worksheet, int sourceRow, int sourceColumn, int startRow, int startColumn, int endRow, int endColumn, string valueToCopied)
        {
            //TarunGupta1
            try
            {

                // Copy the source cell
                Excel.Range sourceCell = worksheet.Cells[sourceRow, sourceColumn];
                sourceCell.Copy();

                // Paste it to the destination range
                for (int row = startRow; row <= endRow; row++)
                {
                    for (int col = startColumn; col <= endColumn; col++)
                    {
                        Excel.Range destinationCell = worksheet.Cells[row, col];
                        if (valueToCopied != null)
                        {
                            destinationCell.Value = valueToCopied;
                        }
                        else
                        {
                            destinationCell.PasteSpecial(Excel.XlPasteType.xlPasteValues);

                        }
                    }
                }

                // Clear the clipboard after pasting
                Excel.Application excelApp = (Excel.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");
                excelApp.CutCopyMode = 0;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }

        }
        static void MoveScriptFile(string sourceFilePath, string destinationFolderPath, string scriptFileName)
        {
            try
            {
                if (File.Exists(sourceFilePath))
                {
                    string destinationScriptFilePath = Path.Combine(destinationFolderPath, scriptFileName);

                    //Directory.CreateDirectory(destinationFolderPath);
                    if (!File.Exists(destinationScriptFilePath))
                    {
                        File.Copy(sourceFilePath, destinationScriptFilePath);
                    }
                    Console.WriteLine("Script file moved successfully!");
                    CreateLogs("Script file moved successfully!", logFilePath);
                }
                else
                {
                    Console.WriteLine("Source script file does not exist.");
                    CreateLogs("Source script file does not exist.", logFilePath);
                }
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed($"An error occurred: {ex.Message}");
                CreateLogs($"An error occurred: {ex.Message}", logFilePath);
            }
        }

        public static void ExecuteBatch(string batchFilePath)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo(batchFilePath)
            {
                // Set the working directory if needed
                WorkingDirectory = inputFolder,
                // Redirect the standard output and error to be able to read them
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            // Create and configure the process
            Process process = new Process
            {
                StartInfo = processInfo
            };

            try
            {
                // Start the process
                process.Start();

                // Read the output and error streams
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                // Wait for the process to exit
                process.WaitForExit();

                // Display the output and error
                Console.WriteLine("Output:");
                Console.WriteLine(output);
                CreateLogs("Output: ", logFilePath);

            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed($"An error occurred: {ex.Message}");
                CreateLogs($"An error occurred: {ex.Message}", logFilePath);
            }
        }


        static void RunScript(string scriptFilePath)
        {
            try
            {
                Process process = new Process();
                // Directory.SetCurrentDirectory(inputFolder);
                //process.StartInfo.WorkingDirectory = inputFolder;
                process.StartInfo.FileName = "cmd.exe";


                process.StartInfo.Arguments = $"/c \"{scriptFilePath}\"";

                process.Start();
                //string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                //  Console.WriteLine(output);
                //process.Close();
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("An error occurred while reading the CSV file: " + ex.Message);
                CreateLogs("An error occurred while reading the CSV file: " + ex.Message, logFilePath);
            }
        }

        static void MoveFileToNewFolder(string sourceFilePath, string destinationFilePath)
        {
            try
            {
                if (File.Exists(sourceFilePath))
                {
                    // Directory.CreateDirectory(Path.GetDirectoryName(destinationFilePath));
                    if (!File.Exists(destinationFilePath))
                    {
                        File.Move(sourceFilePath, destinationFilePath);
                    }

                    Console.WriteLine("File moved successfully!");
                    CreateLogs("File moved successfully!", logFilePath);
                }
                else
                {
                    Console.WriteLine("Source file does not exist.");
                    CreateLogs("Source file does not exist.", logFilePath);
                }
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("An error occurred while reading the CSV file: " + ex.Message);
                CreateLogs("An error occurred while reading the CSV file: " + ex.Message, logFilePath);
            }
        }
        static void MoveFiles(string filePaths)
        {

            string[] arr = args.Split('#');
            string sourceFolder = arr[0];
            string destFolder = outputFolder;
            try
            {
                // Ensure the source folder exists
                if (!Directory.Exists(sourceFolder))
                {
                    Console.WriteLine("Source folder does not exist.");
                    return;
                }

                // Ensure the destination folder exists, if not create it
                if (!Directory.Exists(destFolder))
                {
                    Directory.CreateDirectory(destFolder);
                }

                // Get all files from the source folder
                string[] files = Directory.GetFiles(sourceFolder);

                foreach (string filePath in files)
                {
                    // Get the file name
                    string fileName = Path.GetFileName(filePath);

                    // Define destination file path
                    string destFilePath = Path.Combine(destFolder, fileName);

                    // Move (cut and paste) the file
                    File.Move(filePath, destFilePath);

                    Console.WriteLine($"Moved: {fileName}");
                }

                Console.WriteLine("All files have been moved successfully.");
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("An error occurred: " + ex.Message);
            }
        }
        //static void MoveFileToNewFolder(string sourceFolderPath, string destinationFolderPath, string fileName)
        //{
        //    // Get the list of files in the source folder
        //    //string[] sourceFiles = Directory.GetFiles(sourceFolderPath);

        //    // Check if any files are found
        //    //if (sourceFiles.Length > 0)
        //   // {
        //        // Get the first file found
        //       // string sourceFilePath = sourceFiles[0];

        //        // Combine the destination folder path and file name to get the full destination file path
        //        string destinationFilePath = Path.Combine(destinationFolderPath, Path.GetExtension(sourceFolderPath));

        //        // Create the destination folder if it doesn't exist
        //        //Directory.CreateDirectory(Path.GetDirectoryName(destinationFilePath));

        //        // Move the file to the new folder
        //        File.Move(sourceFolderPath, destinationFilePath);

        //        Console.WriteLine("File moved successfully!");
        //   // }

        //}

        public static System.Data.DataTable DataFromCsvFile(string csvFile)
        {
            System.Data.DataTable dataTable = new System.Data.DataTable();

            try
            {
                using (StreamReader reader = new StreamReader(csvFile))
                {
                    // Read the header line to get column names
                    string[] headers = reader.ReadLine().Split(',');

                    foreach (string header in headers)
                    {
                        dataTable.Columns.Add(header);
                    }

                    while (!reader.EndOfStream)
                    {
                        string[] data = reader.ReadLine().Split(',');
                        DataRow row = dataTable.NewRow();

                        for (int i = 0; i < data.Length; i++)
                        {
                            row[i] = data[i];
                        }

                        dataTable.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                exitcode = 3;
                // Handle any exceptions that may occur during file reading
                LogWithRed("An error occurred while reading the CSV file: " + ex.Message);
                CreateLogs("An error occurred while reading the CSV file: " + ex.Message, logFilePath);
            }
            return dataTable;
        }

        static void ProcessIndividualFile(string inputFolder, string outputFolder, string fileToFunction)
        {
            Console.WriteLine($"Processing individual file: {fileToFunction} from {inputFolder} to {outputFolder}");
            CreateLogs($"Processing individual file: {fileToFunction} from {inputFolder} to {outputFolder}", logFilePath);

            Dictionary<string, List<Action<string>>> keywordFunctions = ReadConfig(fileToFunction);

            IterateFilesInFolder(inputFolder, keywordFunctions, outputFolder);
        }

        static void ProcessAllFiles(string inputFolder, string outputFolder, string functionName)
        {
            Console.WriteLine($"Processing all files in {inputFolder} using function: {functionName} to {outputFolder}");
            CreateLogs($"Processing all files in {inputFolder} using function: {functionName} to {outputFolder}", logFilePath);

            string[] pairs = functionName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string pair in pairs)
            {
                Action<string> function = GetFunctionByName(functionName);
                IterateAllFilesInFolder(inputFolder, function, outputFolder);
            }
        }

        static Dictionary<string, List<Action<string>>> ReadConfig(string configString)
        {
            Dictionary<string, List<Action<string>>> keywordFunctions = new Dictionary<string, List<Action<string>>>();
            try
            {
                // Check if the config file exists
                string[] pairs = configString.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string pair in pairs)
                {
                    // Split each pair into filename and function
                    string[] parts = pair.Trim().Split(':');

                    if (parts.Length == 2)
                    {
                        // Assuming the first part is the filename and the second part is the function name
                        string filename = parts[0].Trim();
                        filename = ReplaceDateTimePlaceholders(filename);
                        string[] functionNames = parts[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string functionName in functionNames)
                        {
                            // Create an Action based on the function name
                            Action<string> function = GetFunctionByName(functionName);

                            // Add to the dictionary
                            // keywordFunctions[filename].Add(function);
                            if (keywordFunctions.ContainsKey(filename))
                            {
                                keywordFunctions[filename].Add(function);
                            }
                            else
                            {
                                // If the key is not present, create a new list and add the function
                                keywordFunctions[filename] = new List<Action<string>> { function };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exitcode = 5;
                LogWithRed("An error occurred while reading the CSV file: " + ex.Message);
                CreateLogs("An error occurred while reading the CSV file: " + ex.Message, logFilePath);
            }
            return keywordFunctions;
        }
        static Dictionary<string, string> ReadConfignames(string configString)
        {
            Dictionary<string, string> keywordFunctions = new Dictionary<string, string>();

            try
            {
                // Check if the config file exists
                string[] pairs = configString.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string pair in pairs)
                {
                    // Split each pair into filename and function
                    string[] parts = pair.Trim().Split(':');

                    if (parts.Length == 2)
                    {
                        // Assuming the first part is the filename and the second part is the function name
                        string filename = parts[0].Trim();
                        string functionNames = parts[1].Trim();

                        if (!keywordFunctions.ContainsKey(filename))
                        {
                            keywordFunctions[filename] = functionNames;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exitcode = 5;
                LogWithRed("An error occurred while reading the CSV file: " + ex.Message);
                CreateLogs("An error occurred while reading the CSV file: " + ex.Message, logFilePath);
            }
            return keywordFunctions;
        }
        private static void InitializeLogFile()
        {
            // Get the current date and time
            DateTime currentDate = DateTime.Now;

            // Create a filename with the current date and time
            logFilename = $@"Logs\Log_{currentDate.ToString("yyyyMMdd_HHmmss")}.txt";

            try
            {
                // Create the log file
                using (StreamWriter writer = File.CreateText(logFilename))
                {
                    writer.WriteLine($"--- Log file created on {currentDate} ---");
                }

                Console.WriteLine("Log file created: " + logFilename);
                CreateLogs("Log file created: " + logFilename, logFilePath);
            }
            catch (Exception ex)
            {
                // If there is an error, display the error message
                LogWithRed("Error creating log file: " + ex.Message);
                CreateLogs("Error creating log file: " + ex.Message, logFilePath);
            }
        }
        private static void Log(string logMessage)
        {
            try
            {
                using (StreamWriter writer = File.AppendText(logFilename))
                {
                    writer.WriteLine(logMessage);
                }
            }
            catch (Exception ex)
            {
                // If there is an error, display the error message
                LogWithRed("Error writing to log file: " + ex.Message);
                CreateLogs("Error writing to log file: " + ex.Message, logFilePath);
            }
        }


        static Action<string> GetFunctionByName(string functionName)
        {
            // functionName=functionName.ToLower();
            // Implement a mapping from function names to corresponding functions
            switch (functionName)
            {
              
            case "ConvertColValueToNumber":
                    return ConvertColValueToNumber;
                case "FidelityConvertDatToText":
                    return FidelityFileConverter.ConvertDatToCsv;
                case "UpdatedConvertColValuetoNumber":
                    return UpdatedConvertColValuetoNumber;
                case "ConvertXmlToCSV":
                    return XMLService.ConvertXmlToCSV;
                case "UpdateValueAccordingToCondition":
                    return UpdateValueAccordingToCondition;
                case "UpdateColumnValueForTicker":
                    return UpdateColumnValueForTicker;
                case "RearrangeCsvColumns":
                    return RearrangeCsvColumns;
                case "ConvertExcelToxls":
                    return ConvertExcelToxls;
                case "ConvertExcelToCSV":
                    return ConvertExcelToCSV;
                case "ConvertexttoCol":
                    return ConvertexttoCol;
                case "clearformat":
                    return ClearFormat;
                case "clearformatall":
                    return ClearFormatfast;
                case "ClearFormatParticularColumn":
                    return ClearFormatParticularColumn;
                case "changeextension":
                    return changeextension;
                case "copyfile":
                    return Copyfile;
                case "CutAndPasteFile":
                    return CutAndPasteFile;
                case "pershing":
                    return Pershing;
                case "comerica":
                    return Comerica;
                case "unzipfile":
                    return UnzipFiles;
                case "pnc":
                    return PNC;
                case "jpmswap":
                    return JPMSwap;
                case "Movesheettofirst":
                    return MoveSheetToFirst;
                case "converttonumber":
                    return ConvertColValuetoNumber;
                case "FindCellValueAndReplace":
                    return FindCellValueAndReplace;
                case "DeleteWorksheet":
                    return DeleteWorksheet;
                case "FilterAndDeleteRows":
                    return FilterAndDeleteRows;
                case "FilterAndDeleteRowsStartFromRow":
                    return FilterAndDeleteRowsStartFromRow;
                case "FilterAndDeleteRowsListStartFromRow":
                    return FilterAndDeleteRowsListStartFromRow;
                case "AppendDataToFile":
                    return AppendDataToFile;
                case "ChangeColumnsToDate":
                    return ChangeColumnsToDate;
                case "PartialFilter":
                    return PartialFilter;
                case "DeleteIfFound":
                    return DeleteIfPresent;
                case "DeleteRowsInRange":
                    return DeleteRowsInRange;
                case "FilterAndDeleteRowsIfContains":
                    return FilterAndDeleteRowsIfContains;
                case "FilterIfContainsStartFromRow":
                    return FilterIfContainsStartFromRow;
                case "FindCellValueAndReplaceInColumn":
                    return FindCellValueAndReplaceInColumn;
                case "ReplaceColumnNameInHeader":
                    return ReplaceColumnNameInHeader;
                case "FindAndReplaceCharacterInColumn":
                    return FindAndReplaceCharacterInColumn;
                case "FilterAndDeleteRowsIfFoundStartFromRow":
                    return FilterAndDeleteRowsIfFoundStartFromRow;
                case "AppendZeroToBeginingOfCell":
                    return AppendZeroToBeginingOfCell;
                case "MoveColumnToFirstPosition":
                    return MoveColumnToFirstPosition;
                case "InsertBlankColumn":
                    return InsertBlankColumn;
                case "InsertBlankRow":
                    return InsertBlankRow;
                case "PerformDivisionBetweenTwoColumn":
                    return PerformDivisionBetweenTwoColumn;
                case "DeleteAllFilesInFolder":
                    return DeleteAllFilesInFolder;
                case "SetCellValue":
                    return SetCellValue;
                case "CopyRowFromOneFileToOther":
                    return CopyRowFromOneFileToOther;
                case "ConvertCommaToColumns":
                    return ConvertCommaToColumns;
                case "FilterAndReplaceInOtherColumn":
                    return FilterAndReplaceInOtherColumn;
                case "FilterAndDeleteRowsListIfContainsStartFromRow":
                    return FilterAndDeleteRowsListIfContainsStartFromRow;
                case "ClearLeadingTrailingEmptyStrings":
                    return ClearLeadingTrailingEmptyStrings;
                case "CutPdfFiles":
                    return CutPdfFiles;
                case "DeletePdfFiles":
                    return DeletePdfFiles;
                case "FilterAndCopyPasteInOtherColumn":
                    return FilterAndCopyPasteInOtherColumn;
                case "DeleteColumnsWithHeaderTextFromExcel":
                    return DeleteColumnsWithHeaderTextFromExcel;
                case "DeleteWorksheetExcept":
                    return DeleteWorksheetExcept;
                case "CopyFirstWorksheetToNewFileWithClearFormat":
                    return CopyFirstWorksheetToNewFileWithClearFormat;
                case "CopyColumnValuesBasedOnCriteria":
                    return CopyColumnValuesBasedOnCriteria;
                case "SubtractAndPasteValues":
                    return SubtractAndPasteValues;
                case "FilterAndDeleteUsingLastUsedRowStartFromRow":
                    return FilterAndDeleteUsingLastUsedRowStartFromRow;
                case "OptimizedFilterAndDelete":
                    return OptimizedFilterAndDelete;
                case "ReplaceCharacterWithEmptyInColumn":
                    return ReplaceCharacterWithEmptyInColumn;
                case "ClearLeadingTrailingEmptyStringsOfParticularColumn":
                    return ClearLeadingTrailingEmptyStringsOfParticularColumn;
                case "ConvertXmlToXls":
                    return ConvertXmlToXls;
                case "RunBatchFile":
                    return RunBatchFile;
                case "AppendAllColumns":
                    return AppendAllColumns;
                case "DivideColumnValues":
                    return DivideColumnValues;
                case "ConvertTxtContainingBackTicksToCsv":
                    return ConvertTxtContainingBackTicksToCsv;
                case "DeleteFilesWithExtension":
                    return DeleteFilesWithExtension;
                case "ConvertAllDatToTxt":
                    return ConvertAllDatToTxt;
                case "ConvertColValueUptoParticularDecimals":
                    return ConvertColValueUptoParticularDecimals;
                case "DeleteRowsBasedOnSearchText":
                    return DeleteRowsBasedOnSearchText;
                case "DeleteRowsExceptHeaderAndLatestDate":
                    return DeleteRowsExceptHeaderAndLatestDate;
                case "CopyAndModifyValues":
                    return CopyAndModifyValues;
                case "CopyFromOneColumnToAnotherIfSearhTextMatch":
                    return CopyFromOneColumnToAnotherIfSearhTextMatch;
                case "DeleteRowsIfNotDouble":
                    return DeleteRowsIfNotDouble;
                case "CountAndPrintDuplicatesInColumn":
                    return CountAndPrintDuplicatesInColumn;
                case "CutAndPasteCell":
                    return CutAndPasteCell;
                case "CutAndPasteBasedOnCondition":
                    return CutAndPasteBasedOnCondition;
                case "FilterAndDeleteRowsListFast":
                    return FilterAndDeleteRowsListFast;
                case "ConvertTxtContainingBackTicksToCsvFast":
                    return ConvertTxtContainingBackTicksToCsvFast;
                case "RunExe":
                    return RunExe;
                case "MoveFiles":
                    return MoveFiles;
                case "ConvertColumnToNumberInCSV":
                    return ConvertColumnToNumberInCSV;
                case "InsertBlankRowAtTop":
                    return InsertBlankRowAtTop;
                case "FilterAndDeleteIfFoundRowsListFast":
                    return FilterAndDeleteIfFoundRowsListFast;
                case "DeleteRowsBasedOnSearchTextList":
                    return DeleteRowsBasedOnSearchTextList;
                case "DeleteRowsBasedOnSearchTextListIfContains":
                    return DeleteRowsBasedOnSearchTextListIfContains;
                case "ConvertCsvTxtToCsv":
                    return ConvertCsvTxtToCsv;
                case "FilterAndReplaceInOtherColumnIfContains":
                    return FilterAndReplaceInOtherColumnIfContains;
                case "ConvertPipeToColumns":
                    return ConvertPipeToColumns;
                case "MergeCsvFiles":
                    return MergeCsvFiles;
                case "TrimSpacesInCsv":
                    return TrimSpacesInCsv;
                case "MoveRowsToTopBasedOnColumnValue":
                    return MoveRowsToTopBasedOnColumnValue;
                case "AutoExpandColumn":
                    return AutoExpandColumn;
                case "FormatColumnsWithDecimalConfigrable":
                    return FormatColumnsWithDecimalConfigrable;

                // Tarun's
                case "ChangeColumnsToDateNew":
                    return ChangeColumnsToDateNew;
                case "FilterAndDeleteRowsBelowDate":
                    return FilterAndDeleteRowsBelowDate;
                case "FilterAndPerformMultiplication":
                    return FilterAndPerformMultiplication;
                case "FilterAndPerformSubstraction":
                    return FilterAndPerformSubstraction;
                case "FilterAndDeleteArgumentRowsList":
                    return FilterAndDeleteArgumentRowsList;
                case "LW_JPM_UpdateDataFromOtherFile":
                    return LW_JPM_UpdateDataFromOtherFile;
                case "SetCellValueInGridRange":
                    return SetCellValueInGridRange;
                case "DeleteBlankRows":
                    return DeleteBlankRows;
                case "ConvertexttoColWithSameExtension":
                    return ConvertexttoColWithSameExtension;
                case "SwapColumns":
                    return SwapColumns;
                case "DeleteColumnsInRange":
                    return DeleteColumnsInRange;
                case "FilterAndDeleteColumnsByHeader":
                    return FilterAndDeleteColumnsByHeader;
                case "AddCloumnUsingLookupFromReferenceFile":
                    return AddCloumnUsingLookupFromReferenceFile;
                case "ReplaceDateTimePattern":
                    return ReplaceDateTimePattern;
                case "Threading_FilterAndDeleteRowsListIfContainsStartFromRow":
                    return Threading_FilterAndDeleteRowsListIfContainsStartFromRow;
                case "Threading_FilterAndDeleteArgumentRowsList":
                    return Threading_FilterAndDeleteArgumentRowsList;
                case "ConvertFixedWidthToCsv_McMorganPosition":
                    return ConvertFixedWidthToCsv_McMorganPosition;
                case "DeleteBelowRowsAfterSearchingText":
                    return DeleteBelowRowsAfterSearchingText;
                case "FindCellValueAtSourceColumnAndReplaceInTargetColumn":
                    return FindCellValueAtSourceColumnAndReplaceInTargetColumn;
                case "DeleteRowsIfOtherColumnAfterFilterContainsSpecificText":
                    return DeleteRowsIfOtherColumnAfterFilterContainsSpecificText;
                case "ConvertexttoCol_Optimized":
                    return ConvertexttoColInMemory;
                case "ConvertexttoColInMemoryCSVFiles":
                    return ConvertexttoColInMemoryCSVFiles; 
                case "FilterAndDeleteRowsListIfContainsStartFromRow_Optimized":
                    return FilterAndDeleteRowsListIfContainsStartFromRow_Optimized;
                case "AppendDataToFile_Optimized":
                    return AppendDataToFile_Optimized;
                case "FilterAndDeleteArgumentRowsList_Optimized":
                    return FilterAndDeleteArgumentRowsList_Optimized;
                case "FilterAndDeleteRowsListIfContainsStartFromRow_Optimized_Crestline":
                    return FilterAndDeleteRowsListIfContainsStartFromRow_Optimized_Crestline;
                case "FilterAndReplaceValueInColumnList_Optimized":
                    return FilterAndReplaceValueInColumnList_Optimized;
                case "ReplaceCommasWithBlanks":
                    return ReplaceCommasWithBlanks;
                case "ModifySymbolAndULSymbolBasedOnDescription":
                    return ModifySymbolAndULSymbolBasedOnDescription;
                case "UnmergeCells":
                    return UnmergeCells;
                case "CopyColumnValues":
                    return CopyColumnValues;
                case "CopyFormatToEntireSheet":
                    return CopyFormatToEntireSheet;
                case "CopyFormatToEFindCellValueAtSourceColumnAndAppendValueInTargetColumnntireSheet":
                    return FindCellValueAtSourceColumnAndAppendValueInTargetColumn;
                case "FilterAndReplaceInOtherColumn_V2":
                    return FilterAndReplaceInOtherColumn_V2;
                case "ConvertCellsToNumberFormat":
                    return ConvertCellsToNumberFormat;
                case "AppendDataToFile_V2":
                    return AppendDataToFile_V2;
                case "AppendZeroToBeginingOfCell_V2":
                    return AppendZeroToBeginingOfCell_V2;
                case "UpdatedConvertColValuetoNumber_V2":
                    return UpdatedConvertColValuetoNumber_V2;
                case "ChangeColumnsToDateNew_V2":
                    return ChangeColumnsToDateNew_V2;
                case "FilterAndReplaceInOtherColumn_SSOMS":
                    return FilterAndReplaceInOtherColumn_SSOMS;
               
                    

                //Bhavya's
                case "ConvertColValueToTwoDecimals":
                    return ConvertColValueToTwoDecimals;
                case "MergeCsvFilesToExcel":
                    return MergeCsvFilesToExcel;
                case "AlignColumnData":
                    return AlignColumnData;
                case "RemoveColorsFromColumns":
                    return RemoveColorsFromColumns;
                case "FormatColumnsWithThousandSeparator":
                    return FormatColumnsWithThousandSeparator;
                case "CalculatePercentageDelta":
                    return CalculatePercentageDelta;
                case "CopyColumnData":
                    return CopyColumnData;
                case "FillColumnWithZero":
                    return FillColumnWithZero;
                case "SumColumnsAndAddTotal":
                    return SumColumnsAndAddTotal;
                case "ConvertFixedWidthToCsv_McMorganTransaction":
                    return ConvertFixedWidthToCsv_McMorganTransaction;
                // Komal's
                case "FindCellValueAndReplaceInColumn_Multiple":
                    return FindCellValueAndReplaceInColumn_Multiple;
                case "CopyColumn":
                    return CopyColumn;
                case "AddNewColumn":
                    return AddNewColumn;
                case "ApplyFormulaOnColumn":
                    return ApplyFormulaOnColumn;
                case "FilterAndDeleteRowsListIfFoundStartFromRow":
                    return FilterAndDeleteRowsListIfFoundStartFromRow;
                case "AppendDataToColumn":
                    return AppendDataToColumn;
                case "ConvertTSVToCSV":
                    return ConvertTSVToCSV;

                case "UpdateCYMIFile":
                    return UpdateCYMIFile;
                case "SetColumnFormat":
                    return SetColumnFormat;
                case "SortRowsByColumn":
                    return SortRowsByColumn;
                case "FindCellValueAndReplaceIfContains":
                    return FindCellValueAndReplaceIfContains;
                case "MergeMultipleFiles":
                    return MergeMultipleFiles;
                case "RenameFilesCondition":
                    return RenameFilesCondition;
                case "Deleterows":
                    return Deleterows;
                case "PerformSubstraction":
                    return PerformSubstraction;
                case "deleteRowsByDateRange":
                    return deleteRowsByDateRange;
                default:
                    return null;
            }
        }

        public static void FormatColumnsWithDecimalConfigrable(string filePath)
        {
            string modifiedFileFolder = "";
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = null;
            excelApp.DisplayAlerts = false;
            Excel.Worksheet worksheet = null;
            try
            {
                workbook = excelApp.Workbooks.Open(filePath);
            }
            catch (Exception ex)
            {
                LogWithRed("Error opening Excel file: " + ex.Message);
                return;
            }

            try
            {
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;

                worksheet = (Excel.Worksheet)workbook.Sheets[1];

                string columnNumbers = args.ToString();
                string[] seprator = columnNumbers.Split('|');
                int decimalPlace = int.Parse(seprator[0]);
                string columnNo = seprator[1];
                string[] columns = columnNo.Split('#');


                // Build the format string based on the number of decimal places
                string decimalFormat = new string('0', decimalPlace); // Creates "00" for 2, "0000" for 4, etc.
                //string numberFormat = $"#,##0.{decimalFormat};(#,##0.{decimalFormat});\"-\"";
                string numberFormat = $"#,##0.{decimalFormat};(#,##0.{decimalFormat});\"-\"";


                foreach (string col in columns)
                {
                    if (int.TryParse(col, out int columnNumber) && columnNumber > 0)
                    {
                        Excel.Range columnRange = worksheet.Columns[columnNumber];
                        columnRange.NumberFormat = numberFormat; // Applying the 1000 separator format for both integers and decimal numbers
                    }
                }

                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close(false);
                excelApp.Quit();

                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        static void TrimSpacesInCsv(string inputFilePath)
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fname = Path.GetFileNameWithoutExtension(inputFilePath);
            string extension = Path.GetExtension(inputFilePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }
            string csvFilePath = outputFolder + @"\" + fname + extension;
            try
            {
                // Read all lines from the input CSV file
                string[] csvLines = File.ReadAllLines(inputFilePath);

                using (StreamWriter outputFile = new StreamWriter(csvFilePath, false)) // False to overwrite if the file exists
                {
                    foreach (string line in csvLines)
                    {
                        // Split the line by commas (CSV format) and trim spaces from each value
                        string[] columns = line.Split(',');

                        for (int i = 0; i < columns.Length; i++)
                        {
                            columns[i] = columns[i].Trim(); // Trim leading and trailing spaces
                        }

                        // Join the trimmed columns back into a CSV line
                        string trimmedLine = string.Join(",", columns);
                        outputFile.WriteLine(trimmedLine); // Write the trimmed line to the output file
                    }
                }

                Console.WriteLine("Leading and trailing spaces removed successfully from CSV file.");
                CreateLogs("Leading and trailing spaces removed successfully from CSV file.", logFilePath);
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("An error occurred: " + ex.Message);
                CreateLogs("An error occurred: " + ex.Message, logFilePath);
            }
        }


        public static void MoveRowsToTopBasedOnColumnValue(string filePath)
        {
            string[] arr = args.Split('#');
            int targetColumn = int.Parse(arr[0]);
            string textValue = arr[1];
            string inputFolderPath = inputFolder;
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }
            string csvFilePath = outputFolder + @"\" + fname + extension;

            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;
            excelApp.DisplayAlerts = false;

            try
            {
                workbook = excelApp.Workbooks.Open(filePath);
                worksheet = (Excel.Worksheet)workbook.Sheets[1];
                Excel.Range usedRange = worksheet.UsedRange;

                int lastRow = usedRange.Rows.Count;
                int currentRow = 1; // Start at the first row for moving

                for (int i = 1; i <= lastRow; i++)
                {
                    Excel.Range cell = (Excel.Range)worksheet.Cells[i, targetColumn];
                    if (cell.Value != null && cell.Value.ToString().Equals(textValue, StringComparison.OrdinalIgnoreCase))
                    {
                        Excel.Range currentRowRange = worksheet.Rows[i];
                        Excel.Range topRowRange = worksheet.Rows[currentRow];

                        // Copy the row to the top and then delete the original
                        currentRowRange.Copy(topRowRange);
                        currentRowRange.Delete(Excel.XlDeleteShiftDirection.xlShiftUp);

                        currentRow++; // Move to the next row at the top
                        i--; // Since the row count is shifted up, decrement i to adjust for this
                    }
                }

                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
            }
            finally
            {
                methodCount++;
                workbook?.Close(false);
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        static void MergeCsvFiles(string filePath)
        {
            string inputFolderPath = inputFolder;
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }
            string csvFilePath = outputFolder + @"\" + fname + extension;
            try
            {
                string[] csvFiles = Directory.GetFiles(inputFolderPath, "*.csv");

                // Check if there are any CSV files in the folder
                if (csvFiles.Length == 0)
                {
                    Console.WriteLine("No CSV files found in the directory.");
                    CreateLogs("No CSV files found in the directory.", logFilePath);
                    return;
                }

                using (StreamWriter outputFile = new StreamWriter(csvFilePath, false)) // False to overwrite if the file exists
                {
                    bool isFirstFile = true;

                    foreach (string csvFile in csvFiles)
                    {
                        using (StreamReader inputFile = new StreamReader(csvFile))
                        {
                            string headerLine = inputFile.ReadLine(); // Read the header line
                            if (isFirstFile)
                            {
                                // Write the header only once from the first file
                                outputFile.WriteLine(headerLine);
                                isFirstFile = false;
                            }

                            string line;
                            while ((line = inputFile.ReadLine()) != null)
                            {
                                outputFile.WriteLine(line); // Append the lines to the output file
                            }
                        }
                    }
                }

                Console.WriteLine("CSV files merged successfully into: " + csvFilePath);
                CreateLogs("CSV files merged successfully into: " + csvFilePath, logFilePath);
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("An error occurred: " + ex.Message);
                CreateLogs("An error occurred: " + ex.Message, logFilePath);
            }
        }


        public static void FilterAndDeleteRowsListFast(string filePath)
        {
            try
            {
                string[] arr = args.Split('#');
                string filterColumnName = arr[0];
                string val = arr[1];
                string[] valToKeep = val.Split(';');
                List<string> allowedValues = valToKeep.ToList();
                // Save the workbook
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;

                List<dynamic> filteredRecords;
                List<string> headers;

                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Read();
                    csv.ReadHeader();
                    headers = csv.HeaderRecord.ToList();

                    var records = csv.GetRecords<dynamic>().ToList();

                    filteredRecords = records
                        .Where(r =>
                        {
                            var record = (IDictionary<string, object>)r;
                            return allowedValues.Contains(record[filterColumnName]?.ToString().Trim());
                        })
                        .ToList();
                }

                using (var writer = new StreamWriter(csvFilePath, false))
                using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    if (filteredRecords.Any())
                    {
                        csvWriter.WriteRecords(filteredRecords); // Write records normally
                    }
                    else
                    {
                        // Manually write headers if no records exist
                        csvWriter.WriteField(headers);
                        csvWriter.NextRecord(); // Move to next line
                    }
                }

                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
        }

        public static void FilterAndDeleteIfFoundRowsListFast(string filePath)
        {
            // Split filter column name and values to delete
            try
            {
                string[] arr = args.Split('#');
                string filterColumnName = arr[0];  // Column name to filter
                string val = arr[1];  // Values to match for deletion
                string[] valuesToDelete = val.Split(';');
                List<string> valuesToRemove = valuesToDelete.ToList();

                // Determine the output CSV file path
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);

                if (!string.IsNullOrWhiteSpace(newfilename))
                {
                    fname = newfilename;
                }

                string csvFilePath = Path.Combine(outputFolder, fname + extension);

                List<dynamic> filteredRecords;

                // Read and filter the CSV records
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<dynamic>().ToList();

                    // Filter records where the column value does not match the values to delete
                    filteredRecords = records
                        .Where(r =>
                        {
                            var record = (IDictionary<string, object>)r;
                            // Keep the row if the value in the filterColumnName does NOT match any in valuesToRemove
                            return !valuesToRemove.Contains(record[filterColumnName]?.ToString().Trim());
                        })
                        .ToList();
                }

                // Write the remaining records to a new CSV file
                using (var writer = new StreamWriter(csvFilePath))
                using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csvWriter.WriteRecords(filteredRecords);
                }
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
        }


        //public static void FilterAndDeleteRowsListFast(string filePath)
        //{
        //    string[] arr = args.Split('#');
        //    int filterColumnIndex = int.Parse(arr[0]);
        //    string val = arr[1];
        //    string[] valToKeep = val.Split(';');
        //    List<string> allowedValues = valToKeep.ToList();
        //    using (var workbook = new XLWorkbook(filePath))
        //    {
        //        var worksheet = workbook.Worksheet(1); // Assuming the first sheet
        //        var rows = worksheet.RangeUsed().RowsUsed().ToList();

        //        foreach (var row in rows)
        //        {
        //            var cellValue = row.Cell(filterColumnIndex).GetString();
        //            if (!allowedValues.Contains(cellValue))
        //            {
        //                row.Delete();
        //            }
        //        }
        //        // Save the workbook
        //        string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        //        string fname = Path.GetFileNameWithoutExtension(filePath);
        //        string extension = Path.GetExtension(filePath);
        //        if (newfilename != "")
        //        {
        //            fname = newfilename;
        //        }

        //        string csvFilePath = outputFolder + @"\" + fname + extension;
        //        workbook.SaveAs(csvFilePath);


        //    }
        //}


        public static void CutAndPasteBasedOnCondition(string filePath)
        {
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            string[] arr = args.Split('#');
            string searchColumn = arr[0];
            string checkText = arr[1];
            string cutColumn = arr[2];
            string targetColumn = arr[3];
            try
            {
                // Start Excel and open the workbook
                excelApp = new Excel.Application();
                excelApp.DisplayAlerts = false;
                workbook = excelApp.Workbooks.Open(filePath);

                // Get the worksheet by name
                worksheet = workbook.Sheets[1];

                // Get the last row with data in the search column
                Excel.Range lastCell = worksheet.Cells[worksheet.Rows.Count, searchColumn];
                Excel.Range searchRange = worksheet.Range[worksheet.Cells[1, searchColumn], lastCell.End[Excel.XlDirection.xlUp]];

                // Iterate through the rows in the search column
                foreach (Excel.Range cell in searchRange)
                {
                    // Check if the current cell contains the specified text
                    if (cell.Value != null && cell.Value.ToString() == checkText)
                    {
                        int row = cell.Row;

                        // Get the cell from the cut column (same row) and target column (row above)
                        Excel.Range cutRange = worksheet.Cells[row, cutColumn];
                        Excel.Range pasteRange = worksheet.Cells[row - 1, targetColumn];

                        // Cut the value from the cut column and paste it in the target column one row above
                        cutRange.Cut(pasteRange);
                    }
                }

                // Save the workbook
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;
                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
            }
            finally
            {
                methodCount++;
                // Close the workbook and release Excel resources
                if (workbook != null)
                {
                    workbook.Close(false);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                }
                if (excelApp != null)
                {
                    excelApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                }
            }
        }

        public static void CutAndPasteCell(string filePath)
        {
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;
            string[] arr = args.Split('#');
            string sourceCell = arr[0];
            string destinationCell = arr[1];
            try
            {
                // Start Excel and open the workbook
                excelApp = new Excel.Application();

                excelApp.DisplayAlerts = false;
                workbook = excelApp.Workbooks.Open(filePath);

                // Get the worksheet by name
                worksheet = workbook.Sheets[1];

                // Get the source and destination ranges
                Excel.Range sourceRange = worksheet.Range[sourceCell];
                Excel.Range destinationRange = worksheet.Range[destinationCell];

                // Copy the content from the source cell to the destination cell (keeping destination formatting)
                destinationRange.Value = sourceRange.Value;
                // Clear the content of the source cell, keeping its formatting intact
                sourceRange.ClearContents();

                // Save the workbook
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;
                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
            }
            finally
            {
                methodCount++;
                // Close the workbook and release Excel resources
                if (workbook != null)
                {
                    workbook.Close(false);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                }
                if (excelApp != null)
                {
                    excelApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                }
            }
        }


        public static void DeleteRowsIfOtherColumnAfterFilterContainsSpecificText(string filePath)
        {
            // Initialize Excel application
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;
            excelApp.DisplayAlerts = false;
            string[] arr = args.Split('#');
            int filterColumnIndex = int.Parse(arr[0]);
            string filterText = arr[1];
            int checkColumnIndex = int.Parse(arr[2]);
            string checkText = arr[3];

            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;

            try
            {
                // Open the workbook and the specified sheet
                workbook = excelApp.Workbooks.Open(filePath);
                worksheet = workbook.Sheets[1] as Excel.Worksheet;

                Excel.Range usedRange = worksheet.UsedRange;
                int lastRow = usedRange.Rows.Count;

                // Loop through the rows, starting from the second row (assuming the first is header)
                for (int row = 2; row <= lastRow; row++)
                {
                    Excel.Range filterCell = worksheet.Cells[row, filterColumnIndex] as Excel.Range;
                    Excel.Range checkCell = worksheet.Cells[row, checkColumnIndex] as Excel.Range;

                    // Check if the cell in the filter column contains the filter text
                    if (filterCell != null && filterCell.Value != null && filterCell.Value.ToString().Contains(filterText))
                    {
                        // Check if the corresponding check column contains the specific text
                        if (checkCell.Value == null || checkCell.Value.ToString().Equals(checkText))
                        {
                            // Delete the row if the condition is met
                            Excel.Range rowToDelete = worksheet.Rows[row] as Excel.Range;
                            rowToDelete.Delete(Excel.XlDeleteShiftDirection.xlShiftUp);
                            row--; // Adjust the row counter since rows are shifted up
                            lastRow--; // Reduce the last row count since a row is deleted
                        }
                    }
                }

                // Save the workbook after deleting the rows
                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
            }
            finally
            {
                methodCount++;
                // Cleanup
                workbook?.Close(false);
                excelApp.Quit();
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }


        public static void CountAndPrintDuplicatesInColumn(string filePath)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;
            excelApp.DisplayAlerts = false;
            string[] arr = args.Split('#');
            int columnIndex = int.Parse(arr[0]);

            try
            {
                Dictionary<string, int> duplicatesCount = new Dictionary<string, int>();

                workbook = excelApp.Workbooks.Open(filePath);
                worksheet = (Excel.Worksheet)workbook.Sheets[1];

                Excel.Range usedRange = worksheet.UsedRange;
                Excel.Range columnRange = usedRange.Columns[columnIndex];

                for (int i = 1; i <= columnRange.Rows.Count; i++)
                {
                    Excel.Range cell = (Excel.Range)columnRange.Cells[i, 1];
                    string cellValue = cell.Value?.ToString() ?? string.Empty;

                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        if (duplicatesCount.ContainsKey(cellValue))
                        {
                            duplicatesCount[cellValue]++;
                        }
                        else
                        {
                            duplicatesCount[cellValue] = 1;
                        }
                    }
                }

                // Print duplicates to the console
                Console.WriteLine($"Duplicate values in column {columnIndex}:");
                bool hasDuplicates = false;

                foreach (var entry in duplicatesCount)
                {
                    if (entry.Value > 1)
                    {
                        hasDuplicates = true;
                        // Set text color to red
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{entry.Key}: {entry.Value} times");
                        CreateLogs($"{entry.Key}: {entry.Value} times", logFilePath);
                        // Reset to default color
                        Console.ResetColor();
                    }
                }

                if (!hasDuplicates)
                {
                    Console.WriteLine("No duplicate values found.");
                    CreateLogs("No duplicate values found.", logFilePath);
                }
            }
            catch (System.Exception ex)
            {
                exitcode = 3;
                // Handle exceptions (logging, rethrowing, etc.)
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                // Clean up
                workbook?.Close(false);
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        static void ConvertTSVToCSV(string excelFilePath)
        {
            try
            {
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string fname = Path.GetFileNameWithoutExtension(excelFilePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }
                string csvFilePath = outputFolder + @"\" + fname + ".csv";
                if (File.Exists(csvFilePath))
                {
                    File.Delete(csvFilePath);
                }
                using (var reader = new StreamReader(excelFilePath))
                using (var writer = new StreamWriter(csvFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Replace tabs with commas
                        string csvLine = line.Replace('\t', ',');
                        writer.WriteLine(csvLine);
                    }
                }
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
                throw;
            }
        }

        public static void AppendDataToColumn(string filePath)
        {
            string logmessage = "";
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(filePath);

            // Load Excel application
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1];

            excelApp.DisplayAlerts = false;
            // Extract filter criteria
            string[] arr = args.Split('#');
            int srcColumnIndex = int.Parse(arr[0]);
            int destColumnIndex = int.Parse(arr[1]);

            int startRowIndex = 2; // Starting from row 1

            try
            {
                // Find the last row in the source column
                int lastSrcRow = worksheet.Cells[worksheet.Rows.Count, srcColumnIndex].End(XlDirection.xlUp).Row;
                // Find the last row in the destination column
                int lastDestRow = worksheet.Cells[worksheet.Rows.Count, destColumnIndex].End(XlDirection.xlUp).Row;

                // Loop through each row up to the last used row in the source column
                for (int i = startRowIndex; i <= lastSrcRow; i++)
                {
                    Range srcCell = worksheet.Cells[i, srcColumnIndex];
                    Range destCell = worksheet.Cells[i, destColumnIndex];

                    // Concatenate the values from source and destination cells and store in destination cell
                    if (srcCell.Value != null && destCell.Value != null)
                    {
                        destCell.Value = destCell.Value.ToString() + srcCell.Value.ToString();
                    }
                    else if (srcCell.Value != null)
                    {
                        destCell.Value = srcCell.Value.ToString();
                    }
                    // If the source cell is empty and the destination cell has a value, keep the destination cell value unchanged
                }

                // Save and close the workbook
                workbook.SaveAs(csvFilePath);
                workbook.Close(false);
                excelApp.Quit();
            }
            catch (Exception ex)
            {
                exitcode = 3;
                logmessage += Environment.NewLine + $"Error appending data to column of sheet in file '{filePath}': {ex.Message}";
                LogWithRed(logmessage);
                CreateLogs(logmessage, logFilePath);
            }
            finally
            {
                methodCount++;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }



        public static void FilterAndDeleteRowsListIfFoundStartFromRow(string filePath)
        {
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(filePath);
            //outputFolder += "\\" + fileName;
            //string outputFolder = Path.Combine(Path.GetDirectoryName(filePath), "Output_Files");
            //string outputFile = Path.Combine(outputFolder, fileName);

            // Load Excel application
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[1]; // Assuming the first sheet

            excelApp.DisplayAlerts = false;
            // Extract filter criteria
            string[] arr = args.Split('#');
            int columnIndex = int.Parse(arr[0]);
            string searchText = arr[1];
            string[] searchTextArr = searchText.Split(';');
            List<string> searchTextList = searchTextArr.ToList();
            string headerText = arr[2];
            int startRowIndex = int.Parse(arr[3]);

            // Filter and delete rows
            try
            {
                Excel.Range usedRange = sheet.UsedRange;
                int rowCount = usedRange.Rows.Count;


                for (int rowIndex = startRowIndex; rowIndex <= rowCount; rowIndex++)
                {
                    Excel.Range cell = (Excel.Range)sheet.Cells[rowIndex, columnIndex];
                    string cellContent = cell.Value != null ? cell.Value.ToString() : "";
                    if (searchTextList.Contains(cellContent))
                    {
                        Excel.Range rowToDelete = (Excel.Range)sheet.Rows[rowIndex];
                        rowToDelete.Delete();
                        rowIndex--; // Adjust rowIndex since the next row moves up after deletion
                        rowCount--; // Adjust rowCount after deletion
                    }
                }

                // Save changes
                workbook.SaveAs(csvFilePath);
                workbook.Close();
                excelApp.Quit();

                Console.WriteLine("Rows not containing the specified text in the specified column have been deleted.");
                CreateLogs("Rows not containing the specified text in the specified column have been deleted.", logFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("An error occurred in method: " + ex.Message);
                CreateLogs("An error occurred in method: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        public static void ApplyFormulaOnColumn(string filePath)
        {
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(filePath);

            // Load Excel application
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1]; // Assuming the first sheet

            excelApp.DisplayAlerts = false;
            // Extract filter criteria
            string[] arr = args.Split('#');
            int destColumnIndex = int.Parse(arr[0]);
            int startRowIndex = int.Parse(arr[1]);
            string formula = arr[2].Replace('@', ',');
            //string formula = "=TEXT(DATE(LEFT(A{rowIndex},4),MID(A{rowIndex},5,2),RIGHT(A{rowIndex},2)),\"\"MM/dd/yyyy\"\")";

            try
            {
                // Get the end row index
                int endRowIndex = worksheet.UsedRange.Rows.Count;

                // Loop through each row and apply the formula
                for (int rowIndex = startRowIndex; rowIndex <= endRowIndex; rowIndex++)
                {
                    Excel.Range cell = worksheet.Cells[rowIndex, destColumnIndex];
                    formula = formula.Replace("row", rowIndex.ToString()).Replace('@', ',');

                    cell.Formula = formula;
                }
                workbook.Application.Calculate();
                // Apply the number format to the destination column
                //Excel.Range destColumnRange = worksheet.Range[worksheet.Cells[startRowIndex, destColumnIndex], worksheet.Cells[endRowIndex, destColumnIndex]];
                //destColumnRange.NumberFormat = "MM/dd/yyyy";
                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
                throw;
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }


        public static void AddNewColumn(string filePath)
        {
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(filePath);

            // Load Excel application
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1]; // Assuming the first sheet

            excelApp.DisplayAlerts = false;
            // Extract filter criteria
            string[] arr = args.Split('#');
            string value = arr[0];
            int destColumnIndex = int.Parse(arr[1]);
            int startRowIndex = int.Parse(arr[2]);
            string columnName = arr[3];

            try
            {
                Excel.Range destColumn = worksheet.Columns[destColumnIndex];
                destColumn.Insert(Excel.XlInsertShiftDirection.xlShiftToRight, false);

                // Set the column header
                Excel.Range headerCell = worksheet.Cells[1, destColumnIndex];
                headerCell.Value2 = columnName;

                // Populate the new column with the specified value starting from the startRowIndex
                int endRowIndex = worksheet.UsedRange.Rows.Count;
                for (int rowIndex = startRowIndex; rowIndex <= endRowIndex; rowIndex++)
                {
                    Excel.Range cell = worksheet.Cells[rowIndex, destColumnIndex];
                    cell.Value2 = value;
                }
                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
                throw;
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }

        }


        public static void CopyColumn(string filePath)
        {
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(filePath);

            // Load Excel application
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet xlWorksheet = (Excel.Worksheet)workbook.Sheets[1]; // Assuming the first sheet

            excelApp.DisplayAlerts = false;
            // Extract filter criteria
            string[] arr = args.Split('#');
            int srcColumnIndex = int.Parse(arr[0]);
            int destColumnIndex = int.Parse(arr[1]);
            int startRowIndex = int.Parse(arr[2]);
            string columnName = arr[3];


            // Define the source range
            try
            {
                Excel.Range srcRange = xlWorksheet.Range[
                        xlWorksheet.Cells[startRowIndex, srcColumnIndex],
                        xlWorksheet.Cells[xlWorksheet.Rows.Count, srcColumnIndex]
                    ].EntireColumn;

                // Define the destination range
                Excel.Range destRange = xlWorksheet.Range[
                    xlWorksheet.Cells[startRowIndex, destColumnIndex],
                    xlWorksheet.Cells[xlWorksheet.Rows.Count, destColumnIndex]
                ].EntireColumn;

                // Copy the source column to the destination column
                srcRange.Copy(destRange);

                xlWorksheet.Cells[startRowIndex, destColumnIndex] = columnName;
                // Save the workbook
                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
                throw;
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        public static void FindCellValueAndReplaceInColumn_Multiple(string filePath)
        {
            string[] arr = args.Split('#');
            int columnIndex = int.Parse(arr[0]);
            //string targetValue = arr[1];
            //string replaceValue = arr[2];
            string targetValue = string.Empty;
            string replaceValue = string.Empty;
            // Create an instance of Excel Application
            Application excelApp = new Application();
            excelApp.DisplayAlerts = false;

            // Open the Excel workbook
            Workbook workbook = excelApp.Workbooks.Open(filePath);

            // Get the worksheet
            Worksheet worksheet = (Worksheet)workbook.Sheets[1];
            try
            {
                // Get the used range of the worksheet
                Range usedRange = worksheet.UsedRange;

                // Get the values of the entire column
                Range columnRange = usedRange.Columns[columnIndex];

                // Get the cell values as an array
                object[,] values = (object[,])columnRange.Value;
                // Loopobject[,] values =  through each cell in the column
                for (int j = 1; j < arr.Length; j++)
                {
                    if (arr[j].Split(';')[0].Equals("blank"))
                        targetValue = string.Empty;
                    else
                        targetValue = arr[j].Split(';')[0];
                    replaceValue = arr[j].Split(';')[1];
                    for (int i = 1; i <= values.GetLength(0); i++)
                    {
                        // Check if the cell value matches the target value
                        if (values[i, 1] != null && values[i, 1].ToString() == targetValue)
                        {
                            // Replace the target value with the replace value
                            values[i, 1] = replaceValue;
                        }
                    }
                }

                // Update the column range with the new values
                columnRange.Value = values;

                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);

                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;
                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("An error occurred: " + ex.Message);
                CreateLogs("An error occurred: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close();

                // Quit Excel application
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }



        public static void FindCellValueAtSourceColumnAndReplaceInTargetColumn(string filePath)
        {
            // Parse the arguments
            try
            {
                string[] arr = args.Split('#');
                int sourceColumnIndex = int.Parse(arr[0]);
                int targetColumnIndex = int.Parse(arr[1]);
                string targetValue = arr[2];
                string replaceValue = arr[3];

                // Create an instance of Excel Application
                Application excelApp = new Application();
                excelApp.DisplayAlerts = false;

                // Open the Excel workbook
                Workbook workbook = excelApp.Workbooks.Open(filePath);

                // Get the worksheet
                Worksheet worksheet = (Worksheet)workbook.Sheets[1];

                // Get the used range of the worksheet
                Range usedRange = worksheet.UsedRange;

                // Get the values of the entire source column
                Range sourceColumnRange = usedRange.Columns[sourceColumnIndex];
                Range targetColumnRange = usedRange.Columns[targetColumnIndex];

                // Get the cell values as an array
                object[,] sourceValues = (object[,])sourceColumnRange.Value;
                object[,] targetValues = (object[,])targetColumnRange.Value;

                // Loop through each cell in the source column
                for (int i = 1; i <= sourceValues.GetLength(0); i++)
                {
                    // Check if the cell value in the source column matches the target value
                    if (sourceValues[i, 1] != null && sourceValues[i, 1].ToString().Contains(targetValue))
                    {
                        // Replace the value in the target column at the same row
                        targetValues[i, 1] = replaceValue;
                    }
                }

                // Update the target column range with the new values
                targetColumnRange.Value = targetValues;

                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);

                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;
                workbook.SaveAs(csvFilePath);
                workbook.Close();

                // Quit Excel application
                excelApp.Quit();

                // Clean up
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
        }


        public static void DeleteBelowRowsAfterSearchingText(string filePath)
        {
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(filePath);

            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet worksheet = workbook.ActiveSheet;
            excelApp.DisplayAlerts = false;

            string[] arr = args.Split('#');
            string columnName = arr[0];
            int headerRow = int.Parse(arr[1]);
            string searchText = arr[2];
            string deleteRowCountStr = arr.Length > 3 ? arr[3] : "";
            int deleteRowCount = string.IsNullOrEmpty(deleteRowCountStr) ? -1 : int.Parse(deleteRowCountStr);

            Excel.Range usedRange = worksheet.UsedRange;

            try
            {
                // Find the column index based on the header name
                int columnIndex = -1;
                Excel.Range headerRange = worksheet.Rows[headerRow];
                for (int i = 1; i <= headerRange.Columns.Count; i++)
                {
                    if (headerRange.Cells[1, i].Value != null && headerRange.Cells[1, i].Value.ToString().Trim() == columnName.Trim())
                    {
                        columnIndex = i;
                        break;
                    }
                }

                if (columnIndex == -1)
                {
                    Console.WriteLine("Column not found.");
                    CreateLogs("Column not found.", logFilePath);
                    workbook.Close(false);
                    excelApp.Quit();
                    return;
                }

                // Search for the text in the specified column
                int startRowIndex = headerRow + 1;
                int rowCount = usedRange.Rows.Count;
                for (int rowIndex = startRowIndex; rowIndex <= rowCount; rowIndex++)
                {
                    Excel.Range cell = (Excel.Range)worksheet.Cells[rowIndex, columnIndex];
                    string cellContent = cell.Value != null ? cell.Value.ToString().Trim() : "";

                    if (cellContent.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        // Determine the range of rows to delete
                        int endRowIndex = deleteRowCount == -1 ? rowCount : rowIndex + deleteRowCount - 1;

                        if (endRowIndex > rowCount)
                        {
                            endRowIndex = rowCount;
                        }

                        // Delete rows from endRowIndex to rowIndex (backward deletion)
                        for (int i = endRowIndex; i >= rowIndex; i--)
                        {
                            Excel.Range rowToDelete = (Excel.Range)worksheet.Rows[i];
                            rowToDelete.Delete();
                        }
                        break;
                    }
                }

                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        public static void ConvertFixedWidthToCsv_McMorganPosition(string inputFilePath)
        {
            try
            {
                // Use global elements if available
                string fname = Path.GetFileNameWithoutExtension(inputFilePath);
                string extension = Path.GetExtension(inputFilePath);
                if (!string.IsNullOrEmpty(newfilename))
                {
                    fname = newfilename;
                }

                string csvFilePath = Path.Combine(outputFolder, $"{fname}.csv");

                // Define the column offsets and lengths as per the batch file
                int[] columnStartPositions = new int[] {
                        0, 2, 14, 126, 143, 158, 178, 208, 220, 241, 323, 340, 360, 454, 562, 643, 1520, 1270, 1500, 1512
                };

                int[] columnLengths = new int[] {
                        2, 12, 36, 17, 16, 15, 15, 12, 13, 36, 17, 20, 15, 9, 15, 15, 3, 6, 12, 8
                };

                // Write CSV header
                string[] headers = new string[] {
                        "SN", "Account Number", "Account Name", "Shares/Par Value (Positions)", "Federal Tax Cost (Positions)",
                        "Book Value (Positions)", "Current Price", "CUSIP", "Ticker", "Asset Name", "Par Value/Shares", "Unit Cost",
                        "Original Cost", "Trade Date", "Gain or Loss", "Market Value", "Currency Code", "Broker Number",
                        "ISIN Number", "SEDOL Asset Identifier"
                };

                using (StreamWriter sw = new StreamWriter(csvFilePath))
                {
                    sw.WriteLine(string.Join(",", headers));

                    // Read and process each line
                    foreach (string line in File.ReadLines(inputFilePath))
                    {
                        List<string> rowValues = new List<string>();

                        for (int i = 0; i < columnStartPositions.Length; i++)
                        {
                            int startIndex = columnStartPositions[i];
                            int length = columnLengths[i];

                            if (startIndex + length > line.Length) break;

                            string value = line.Substring(startIndex, length).Trim();
                            rowValues.Add(value);
                        }

                        sw.WriteLine(string.Join(",", rowValues));
                    }
                }

                Console.WriteLine($"CSV file '{csvFilePath}' created successfully.");
                CreateLogs($"CSV file '{csvFilePath}' created successfully.", logFilePath);
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
        }


        public static void ConvertFixedWidthToCsv_McMorganTransaction(string inputFilePath)
        {
            try
            {
                // Use global elements if available
                string fname = Path.GetFileNameWithoutExtension(inputFilePath);
                string extension = Path.GetExtension(inputFilePath);
                if (!string.IsNullOrEmpty(newfilename))
                {
                    fname = newfilename;
                }

                string csvFilePath = Path.Combine(outputFolder, $"{fname}.csv");

                // Define the column offsets and positions as per the batch file
                int[] columnStartPositions = new int[] {
                        0, 239, 127, 135, 153, 203, 274, 465, 725, 2065, 2620, 2608, 2735, 825, 374, 805, 1145, 1265, 1425, 254, 3246, 2421, 2632
                };

                int[] columnLengths = new int[] {
                        12, 12, 8, 8, 5, 36, 20, 3, 20, 20, 12, 12, 31, 20, 20, 20, 20, 20, 20, 20, 48, 3, 12
                };

                // Write CSV header
                string[] headers = new string[] {
                        "Account Number", "Transaction Type", "Trade Date", "Settlement Date", "Asset Name",
                        "Units", "Currency", "Activity Fee", "SEC Fee", "Ticker", "CUSIP", "Broker Name",
                        "Commission", "Rate Per Unit", "Book Value", "Gross Amount", "Local Book Value",
                        "Local Market Value", "Net Cash", "Transaction details", "Corporate Action Type",
                        "Unit Price"
                };

                using (StreamWriter sw = new StreamWriter(csvFilePath))
                {
                    sw.WriteLine(string.Join(",", headers));

                    // Read and process each line
                    foreach (string line in File.ReadLines(inputFilePath))
                    {
                        List<string> rowValues = new List<string>();

                        for (int i = 0; i < columnStartPositions.Length; i++)
                        {
                            int startIndex = columnStartPositions[i];
                            int length = columnLengths[i];

                            if (startIndex + length > line.Length) break;

                            string value = line.Substring(startIndex, length).Trim();

                            // Todo: Use better approach
                            if (columnStartPositions[i] == 153)
                            {
                                rowValues[rowValues.Count - 1] += value;
                            }
                            else
                            {
                                rowValues.Add(value);
                            }
                        }

                        sw.WriteLine(string.Join(",", rowValues));
                    }
                }

                Console.WriteLine($"CSV file '{csvFilePath}' created successfully.");
                CreateLogs($"CSV file '{csvFilePath}' created successfully.", logFilePath);
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
        }

        public static void Threading_FilterAndDeleteArgumentRowsList(string filePath)
        {
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(filePath);

            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet sheet = null;

            try
            {
                // Load Excel application
                excelApp = new Excel.Application();
                workbook = excelApp.Workbooks.Open(filePath);
                sheet = (Excel.Worksheet)workbook.Sheets[1]; // Assuming the first sheet

                excelApp.DisplayAlerts = false;

                int columnIndex;
                string searchText;
                string[] searchTextArr;
                List<string> searchTextList;
                string headerText;
                int startRowIndex;

                if (!string.IsNullOrEmpty(SplitParameter))
                {
                    char[] charArray = SplitParameter.ToCharArray();
                    string[] arr2 = args.Split(charArray[0]);
                    columnIndex = int.Parse(arr2[0]);
                    searchText = arr2[1];
                    searchTextArr = searchText.Split(';');
                    searchTextList = searchTextArr.ToList();
                    headerText = arr2[2];
                    startRowIndex = int.Parse(arr2[3]);
                }
                else
                {
                    string[] arr = args.Split('#');
                    columnIndex = int.Parse(arr[0]);
                    searchText = arr[1];
                    searchTextArr = searchText.Split(';');
                    searchTextList = searchTextArr.ToList();
                    headerText = arr[2];
                    startRowIndex = int.Parse(arr[3]);
                }

                // Filter and delete rows
                Excel.Range usedRange = sheet.UsedRange;
                int rowCount = usedRange.Rows.Count;
                int chunkSize = (rowCount - startRowIndex + 1) / 8;

                // List to hold tasks
                List<Task> tasks = new List<Task>();

                int pasteRow = startRowIndex;

                for (int i = 0; i < 16; i++)
                {
                    int chunkStart = startRowIndex + i * chunkSize;
                    int chunkEnd = (i == 15) ? rowCount : chunkStart + chunkSize - 1;

                    tasks.Add(Task.Run(() =>
                    {
                        for (int rowIndex = chunkStart; rowIndex <= chunkEnd; rowIndex++)
                        {
                            Excel.Range cell = (Excel.Range)sheet.Cells[rowIndex, columnIndex];
                            string cellContent = cell.Value != null ? cell.Value.ToString() : "";
                            cellContent = cellContent.Trim();
                            if (!(SearchTextContainsItems(searchTextList, cellContent) && !cellContent.Equals(headerText)))
                            {
                                Excel.Range sourceRow = (Excel.Range)sheet.Rows[rowIndex];
                                Excel.Range destinationRow = (Excel.Range)sheet.Rows[pasteRow];
                                sourceRow.Copy(destinationRow);
                                pasteRow++;
                            }
                        }
                    }));
                }

                Task.WaitAll(tasks.ToArray());

                // Clear contents of rows below the last pasted row
                if (pasteRow <= rowCount)
                {
                    Excel.Range clearRange = sheet.Rows[pasteRow].Resize[rowCount - pasteRow + 1];
                    clearRange.ClearContents();
                }

                // Save changes
                workbook.SaveAs(csvFilePath);
                Console.WriteLine("Rows not containing the specified text in the specified column have been deleted.");
                CreateLogs("Rows not containing the specified text in the specified column have been deleted.", logFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                // Close and release resources
                if (workbook != null)
                {
                    workbook.Close(false);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                }
                if (sheet != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                }
                if (excelApp != null)
                {
                    excelApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                }
            }
        }

        public static void Threading_FilterAndDeleteRowsListIfContainsStartFromRow(string filePath)
        {
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(filePath);

            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet sheet = null;

            try
            {
                // Load Excel application
                excelApp = new Excel.Application();
                workbook = excelApp.Workbooks.Open(filePath);
                sheet = (Excel.Worksheet)workbook.Sheets[1]; // Assuming the first sheet

                excelApp.DisplayAlerts = false;

                // Extract filter criteria
                string[] arr = args.Split('#');
                int columnIndex = int.Parse(arr[0]);
                string searchText = arr[1];
                string[] searchTextArr = searchText.Split(';');
                List<string> searchTextList = searchTextArr.ToList();
                string headerText = arr[2];
                int startRowIndex = int.Parse(arr[3]);

                // Filter and delete rows
                Excel.Range usedRange = sheet.UsedRange;
                int rowCount = usedRange.Rows.Count;
                int chunkSize = (rowCount - startRowIndex + 1) / 8;

                // List to hold tasks
                List<Task> tasks = new List<Task>();

                int pasteRow = startRowIndex;

                for (int i = 0; i < 16; i++)
                {
                    int chunkStart = startRowIndex + i * chunkSize;
                    int chunkEnd = (i == 15) ? rowCount : chunkStart + chunkSize - 1;

                    tasks.Add(Task.Run(() =>
                    {
                        for (int rowIndex = chunkStart; rowIndex <= chunkEnd; rowIndex++)
                        {
                            Excel.Range cell = (Excel.Range)sheet.Cells[rowIndex, columnIndex];
                            string cellContent = cell.Value != null ? cell.Value.ToString() : "";
                            cellContent = cellContent.Trim();
                            if (!(!(SearchTextContainsItems(searchTextList, cellContent) || cellContent.Equals(headerText))))
                            {
                                Excel.Range sourceRow = (Excel.Range)sheet.Rows[rowIndex];
                                Excel.Range destinationRow = (Excel.Range)sheet.Rows[pasteRow];
                                sourceRow.Copy(destinationRow);
                                pasteRow++;
                            }
                        }
                    }));
                }

                Task.WaitAll(tasks.ToArray());

                // Clear contents of rows below the last pasted row
                if (pasteRow <= rowCount)
                {
                    Excel.Range clearRange = sheet.Rows[pasteRow].Resize[rowCount - pasteRow + 1];
                    clearRange.ClearContents();
                }

                // Save changes
                workbook.SaveAs(csvFilePath);
                Console.WriteLine("Rows not containing the specified text in the specified column have been deleted.");
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
            }
            finally
            {
                methodCount++;
                // Close and release resources
                if (workbook != null)
                {
                    workbook.Close(false);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                }
                if (sheet != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                }
                if (excelApp != null)
                {
                    excelApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                }
            }

        }


        public static void ReplaceDateTimePattern(string filePath)
        {
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            string csvFilePath = outputFolder + @"\" + fname + extension;

            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                workbook = excelApp.Workbooks.Open(filePath);
                worksheet = (Excel.Worksheet)workbook.Sheets[1]; // Assuming the first sheet

                Excel.Range usedRange = worksheet.UsedRange;

                // Extract filter criteria from args
                string[] arr = args.Split('#');
                string columnName = arr[0];
                int columnHeaderRow = int.Parse(arr[1]);

                int columnIndex = -1;

                // Find the column index by header name
                for (int col = 1; col <= usedRange.Columns.Count; col++)
                {
                    Excel.Range cell = worksheet.Cells[columnHeaderRow, col] as Excel.Range;
                    if (cell.Value != null && cell.Value.ToString() == columnName)
                    {
                        columnIndex = col;
                        break;
                    }
                }

                if (columnIndex == -1)
                {
                    throw new Exception("Column not found: " + columnName);
                }

                // Iterate through rows and replace date-time values with date only
                for (int row = columnHeaderRow + 1; row <= usedRange.Rows.Count; row++) // Start from the row below the header
                {
                    Excel.Range cell = worksheet.Cells[row, columnIndex] as Excel.Range;
                    if (cell.Value != null)
                    {
                        string cellContent = cell.Value.ToString();
                        if (cellContent.EndsWith("Z"))
                        {
                            // Remove the "T" and time part from the date-time string
                            string dateOnly = cellContent.Split('T')[0];

                            // Update the cell value with the new formatted value
                            cell.Value = dateOnly;
                        }
                    }
                }
                workbook.SaveAs(csvFilePath);
                Console.WriteLine("Date-time pattern replacement completed in column: " + columnName);
                CreateLogs("Date-time pattern replacement completed in column: " + columnName, logFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                // Clean up Excel objects
                if (workbook != null)
                {
                    workbook.Close(false);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                }
                if (worksheet != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                }
                if (excelApp != null)
                {
                    excelApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                }
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }


        }

        public static void AddCloumnUsingLookupFromReferenceFile(string filePath)
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(filePath);

            string partialFilePath = currentDirectory;
            string[] arr = args.Split('#');
            if (!arr[0].Equals("NA"))
            {
                partialFilePath = currentDirectory + arr[0];
            }

            string partialFileName = arr[1];
            string[] matchingFiles = FindFilesWithPartialName(partialFilePath, partialFileName);
            string referenceFilePath = matchingFiles[0];

            if (!File.Exists(referenceFilePath))
            {
                Console.WriteLine("Reference file does not exist.");
                return;
            }

            // Parse arguments
            int mainFilePrimaryKeyColumnIndex = int.Parse(arr[2]);
            int referenceFileForeignKeyColumnIndex = int.Parse(arr[3]);
            int referenceFileDataToFetchColumnIndex = int.Parse(arr[4]);
            int mainFileStartRowIndex = int.Parse(arr[5]);
            int referenceFileStartRowIndex = int.Parse(arr[6]);
            string accountNameColumnArg = arr[7];
            int accountNameColumnIndex = string.IsNullOrEmpty(accountNameColumnArg) ? -1 : int.Parse(accountNameColumnArg);

            Excel.Application excelApp = null;
            Excel.Workbook mainWorkbook = null;
            Excel.Workbook referenceWorkbook = null;

            try
            {
                // Load Excel application
                excelApp = new Excel.Application();
                mainWorkbook = excelApp.Workbooks.Open(filePath);
                referenceWorkbook = excelApp.Workbooks.Open(referenceFilePath);

                Excel.Worksheet mainSheet = (Excel.Worksheet)mainWorkbook.Sheets[1]; // Assuming the first sheet
                Excel.Worksheet referenceSheet = (Excel.Worksheet)referenceWorkbook.Sheets[1]; // Assuming the first sheet

                excelApp.DisplayAlerts = false;

                // Read reference data into a dictionary
                Dictionary<object, object> referenceData = new Dictionary<object, object>();

                Excel.Range referenceUsedRange = referenceSheet.UsedRange;
                int referenceRowCount = referenceUsedRange.Rows.Count;

                for (int rowIndex = referenceFileStartRowIndex; rowIndex <= referenceRowCount; rowIndex++) // Starting from specified row
                {
                    object key = referenceSheet.Cells[rowIndex, referenceFileForeignKeyColumnIndex].Value;
                    object value = referenceSheet.Cells[rowIndex, referenceFileDataToFetchColumnIndex].Value;
                    if (key != null && !referenceData.ContainsKey(key))
                    {
                        referenceData.Add(key, value);
                    }
                }

                // Determine the column index for the Account Name
                Excel.Range mainUsedRange = mainSheet.UsedRange;
                int mainRowCount = mainUsedRange.Rows.Count;
                if (accountNameColumnIndex == -1)
                {
                    accountNameColumnIndex = mainUsedRange.Columns.Count + 1; // Add new column at the end
                }

                for (int rowIndex = mainFileStartRowIndex; rowIndex <= mainRowCount; rowIndex++) // Starting from specified row
                {
                    object key = mainSheet.Cells[rowIndex, mainFilePrimaryKeyColumnIndex].Value;
                    if (key != null && referenceData.ContainsKey(key))
                    {
                        mainSheet.Cells[rowIndex, accountNameColumnIndex] = referenceData[key];
                    }
                }

                // Save changes
                mainWorkbook.SaveAs(csvFilePath);
                Console.WriteLine("Account Name column added based on the reference file.");
                CreateLogs("Account Name column added based on the reference file.", logFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                // Close and release resources
                if (mainWorkbook != null)
                {
                    mainWorkbook.Close(false);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(mainWorkbook);
                }
                if (referenceWorkbook != null)
                {
                    referenceWorkbook.Close(false);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(referenceWorkbook);
                }
                if (excelApp != null)
                {
                    excelApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                }
                if (mainWorkbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(mainWorkbook);
                if (referenceWorkbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(referenceWorkbook);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }

        }

        public static void FilterAndDeleteColumnsByHeader(string filePath)
        {
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(filePath);

            // Load Excel application
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[1]; // Assuming the first sheet

            excelApp.DisplayAlerts = false;

            // Extract filter criteria
            string[] arr = args.Split('#');
            int headerRowIndex = int.Parse(arr[0]);
            string headerTexts = arr[1];
            string[] headerTextArr = headerTexts.Split(';');
            List<string> headerTextList = headerTextArr.ToList();

            // Get the used range and the total number of columns
            Excel.Range usedRange = sheet.UsedRange;
            int columnCount = usedRange.Columns.Count;
            try
            {

                // Iterate over the columns and delete the ones not matching the header text criteria
                for (int colIndex = columnCount; colIndex >= 1; colIndex--)
                {
                    Excel.Range cell = (Excel.Range)sheet.Cells[headerRowIndex, colIndex];
                    string cellContent = cell.Value != null ? cell.Value.ToString() : "";
                    cellContent = cellContent.Trim();
                    if (!headerTextList.Contains(cellContent))
                    {
                        Excel.Range columnToDelete = (Excel.Range)sheet.Columns[colIndex];
                        columnToDelete.Delete();
                    }
                }

                // Save changes
                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();

                // Release COM objects to avoid memory leaks
                System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

                // Optional: Force garbage collection to free up resources immediately
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            Console.WriteLine("Columns not containing the specified header text have been deleted.");
            CreateLogs("Columns not containing the specified header text have been deleted.", logFilePath);
        }

        public static void DeleteColumnsInRange(string filePath)
        {
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (!string.IsNullOrEmpty(newfilename))
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(filePath);

            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet worksheet = workbook.ActiveSheet;
            excelApp.DisplayAlerts = false;
            string[] arr = args.Split('#');

            int startColIndex = int.Parse(arr[0]);
            int endColIndex = int.Parse(arr[1]);
            Excel.Range usedRange = worksheet.UsedRange;

            try
            {
                // Ensure start column index and end column index are within the range of columns in the worksheet
                if (startColIndex >= 1 && endColIndex <= usedRange.Columns.Count && startColIndex <= endColIndex)
                {
                    // Delete columns starting from endColIndex and work backwards to startColIndex
                    for (int colIndex = endColIndex; colIndex >= startColIndex; colIndex--)
                    {
                        Excel.Range col = (Excel.Range)worksheet.Columns[colIndex];
                        col.Delete();
                    }

                    workbook.SaveAs(csvFilePath);
                }
                else
                {
                    Console.WriteLine("Invalid start column index or end column index.");
                }
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();

                // Release COM objects to avoid memory leaks
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

                // Optional: Force garbage collection to free up resources immediately
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }


        private static int FindColumnByHeader(Excel.Worksheet worksheet, string header, int rowHeaderNumber)
        {
            Excel.Range usedRange = worksheet.UsedRange;
            for (int col = 1; col <= usedRange.Columns.Count; col++)
            {

                Excel.Range cell = (Excel.Range)worksheet.Cells[rowHeaderNumber, col];
                string cellContent = cell.Value != null ? cell.Value.ToString() : "";
                if (cellContent == header)
                {
                    return col;
                }
            }
            return -1; // Not found
        }

        static void SwapColumns(string filePath)
        {
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                string[] arr;
                int startRow1;
                string colHeader1;
                int startRow2;
                string colHeader2;
                int rowHeaderNumber;
                if (SplitParameter != "")
                {
                    char[] charArray = SplitParameter.ToCharArray();




                    arr = args.Split(charArray[0]);
                    startRow1 = int.Parse(arr[0]);
                    colHeader1 = arr[1];
                    startRow2 = int.Parse(arr[2]);
                    colHeader2 = arr[3];
                    rowHeaderNumber = int.Parse(arr[4]);

                }
                else
                {
                    arr = args.Split('#');
                    startRow1 = int.Parse(arr[0]);
                    colHeader1 = arr[1];
                    startRow2 = int.Parse(arr[2]);
                    colHeader2 = arr[3];
                    rowHeaderNumber = int.Parse(arr[4]);
                }

                excelApp = new Excel.Application();
                excelApp.DisplayAlerts = false;

                workbook = excelApp.Workbooks.Open(filePath);
                worksheet = workbook.Sheets[1];

                // Find the columns based on headers
                int col1 = FindColumnByHeader(worksheet, colHeader1, rowHeaderNumber);
                int col2 = FindColumnByHeader(worksheet, colHeader2, rowHeaderNumber);

                if (col1 == -1 || col2 == -1)
                {
                    throw new Exception("One or both column headers not found");
                }

                // Swap columns
                int lastRowCol1 = worksheet.Cells[worksheet.Rows.Count, col1].End(Excel.XlDirection.xlUp).Row;
                int lastRowCol2 = worksheet.Cells[worksheet.Rows.Count, col2].End(Excel.XlDirection.xlUp).Row;
                int lastRow = Math.Max(lastRowCol1, lastRowCol2);


                for (int row = rowHeaderNumber; row <= lastRow; row++)
                {
                    var temp = worksheet.Cells[row, col1].Value;
                    worksheet.Cells[row, col1].Value = worksheet.Cells[row, col2].Value;
                    worksheet.Cells[row, col2].Value = temp;
                }

                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (!string.IsNullOrEmpty(newfilename))
                {
                    fname = newfilename;
                }
                string newFilePath = Path.Combine(outputFolder, fname + extension);

                workbook.SaveAs(newFilePath);

                Console.WriteLine("Columns swapped successfully for " + fname);
                CreateLogs("Columns swapped successfully for " + fname, logFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                if (workbook != null)
                {
                    workbook.Close(false);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                }
                if (excelApp != null)
                {
                    excelApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                }
                if (worksheet != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }


        static void ConvertexttoColWithSameExtension(string excelFilePath)
        {
            string newFilePath = "";
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;
            string[] arr = args.Split('#');
            string delimiter = arr[0];

            try
            {
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string fname = Path.GetFileNameWithoutExtension(excelFilePath);
                string extension = Path.GetExtension(excelFilePath);

                if (!string.IsNullOrEmpty(newfilename))
                {
                    fname = newfilename;
                }
                newFilePath = Path.Combine(outputFolder, fname + extension);

                excelApp = new Excel.Application();
                excelApp.DisplayAlerts = false;

                workbook = excelApp.Workbooks.Open(excelFilePath);
                ClearFormatworkbook(excelFilePath, workbook);

                worksheet = (Excel.Worksheet)workbook.Sheets[1];

                if (File.Exists(newFilePath))
                {
                    // If it does, delete the existing file
                    File.Delete(newFilePath);
                }

                // Specify the range to apply Text to Columns
                Excel.Range range = worksheet.UsedRange;

                // Specify delimiter and other options
                Excel.Range textToColumnsRange = range;

                excelApp.DisplayAlerts = false;
                textToColumnsRange.TextToColumns(
                    textToColumnsRange, // Destination
                    Excel.XlTextParsingType.xlDelimited, // DataType
                    Excel.XlTextQualifier.xlTextQualifierNone, // TextQualifier
                    true, // ConsecutiveDelimiter
                    true, // Tab
                    false, // Semicolon
                    false, // Comma
                    false, // Space
                    true, // Other
                    delimiter, // OtherChar
                    Excel.XlColumnDataType.xlGeneralFormat, // FieldInfo
                    Type.Missing, // DecimalSeparator
                    Type.Missing, // ThousandsSeparator
                    Type.Missing  // TrailingMinusNumbers
                );

                workbook.SaveAs(newFilePath);

                Console.WriteLine("Conversion text to column completed for " + fname);
                CreateLogs("Conversion text to column completed for " + fname, logFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                // Close and release resources
                if (workbook != null)
                {
                    workbook.Close(false);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                }
                if (excelApp != null)
                {
                    excelApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                }
                if (worksheet != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                }

                // Optional: Force garbage collection to free up resources immediately
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            ClearFormat(newFilePath);
            if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
            if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
        }

        private static bool IsRowEmpty(Excel.Range range)
        {
            foreach (Excel.Range cell in range.Cells)
            {
                if (cell.Value2 != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(cell);
                    return false;
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(cell);
            }
            return true;
        }

        static void DeleteBlankRows(string filePath)
        {
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                // Create a new Excel application
                excelApp = new Excel.Application();

                excelApp.DisplayAlerts = false;

                // Open the workbook
                workbook = excelApp.Workbooks.Open(filePath);

                // Get the first worksheet
                worksheet = workbook.Sheets[1]; // Index is 1-based

                // Get the last used row number
                int lastRow = worksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;

                // Loop through each row from the bottom up
                for (int row = lastRow; row >= 1; row--)
                {
                    Excel.Range rowRange = worksheet.Rows[row];

                    // Check if the entire row is empty
                    if (IsRowEmpty(rowRange))
                    {
                        rowRange.Delete();
                    }

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(rowRange);
                }
                lastRow = worksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;

                // Save the workbook
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (!string.IsNullOrEmpty(newfilename))
                {
                    fname = newfilename;
                }
                string csvFilePath = Path.Combine(outputFolder, fname + extension);

                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                // Handle the exception (e.g., log it)
                LogWithRed("An error occurred: " + ex.Message);
                CreateLogs("An error occurred: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                // Ensure the workbook and Excel application are closed properly
                if (workbook != null)
                {
                    workbook.Close(false);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                }
                if (excelApp != null)
                {
                    excelApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                }

                if (worksheet != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                }

                // Optional: Force garbage collection to free up resources immediately
                GC.Collect();
                GC.WaitForPendingFinalizers();
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }


        public static void CopyFromOneColumnToAnotherIfSearhTextMatch(string filePath)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;
            excelApp.DisplayAlerts = false;

            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;

            string[] arr = args.Split('#');

            int sourceColumn = int.Parse(arr[0]);
            int targetColumn = int.Parse(arr[1]);
            string searchText = arr[2];
            try
            {
                // Open the workbook
                workbook = excelApp.Workbooks.Open(filePath);
                worksheet = workbook.Sheets[1];

                Excel.Range usedRange = worksheet.UsedRange;
                int lastRow = usedRange.Rows.Count;

                for (int row = 1; row <= lastRow; row++)
                {
                    Excel.Range sourceCell = worksheet.Cells[row, sourceColumn];
                    Excel.Range targetCell = worksheet.Cells[row, targetColumn];

                    // Check if the source cell contains the search text
                    if (sourceCell.Value != null && sourceCell.Value.ToString().Equals(searchText))
                    {
                        // Replace the search text in the source cell with the target cell value
                        sourceCell.Value = sourceCell.Value.ToString().Replace(searchText, targetCell.Value.ToString());
                    }
                }

                // Save the workbook
                workbook.SaveAs(csvFilePath);
            }
            catch (System.Runtime.InteropServices.COMException comEx)
            {
                exitcode = 3;
                // Handle Excel-specific COM exceptions
                LogWithRed("An Excel-related error occurred: " + comEx.Message);
                CreateLogs("An Excel-related error occurred: " + comEx.Message, logFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                // Handle general exceptions
                LogWithRed("An error occurred: " + ex.Message);
                CreateLogs("An error occurred: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                // Ensure that the workbook and Excel application are properly closed
                if (workbook != null)
                {
                    workbook.Close(false); // Close without saving again, as we already saved in the try block
                }
                excelApp.Quit();
                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }

        }


        public static void SumColumnsAndAddTotal(string filePath)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = null;
            excelApp.DisplayAlerts = false;
            Excel.Worksheet worksheet = null;

            try
            {
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;

                string columnNumbers = args.ToString();

                workbook = excelApp.Workbooks.Open(filePath);
                worksheet = (Excel.Worksheet)workbook.Sheets[1];

                // Split the input columns string into an array of column numbers
                string[] columns = columnNumbers.Split('#');

                // Find the last used row in the worksheet to place the sum below the data
                int lastRow = worksheet.Cells[worksheet.Rows.Count, 1].End(Excel.XlDirection.xlUp).Row;

                foreach (string col in columns)
                {
                    if (int.TryParse(col, out int columnNumber) && columnNumber > 0)
                    {
                        // Get the range of the column to sum
                        Excel.Range columnRange = worksheet.Columns[columnNumber];

                        // Calculate the sum for the column
                        double columnSum = (double)worksheet.Application.WorksheetFunction.Sum(columnRange.Cells);

                        // Place the sum in the next row after the last row of data
                        worksheet.Cells[lastRow + 2, columnNumber].Value = columnSum;

                        // Optional: Format the summed cell with a thousand separator
                        worksheet.Cells[lastRow + 2, columnNumber].NumberFormat = "#,##0.00";
                    }
                }

                // Save the workbook
                workbook.SaveAs(csvFilePath);

            }

            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close(false);
                excelApp.Quit();

                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }


        public static void FillColumnWithZero(string filePath)
        {
            string[] columnIndices = args.Split('#');

            // Initialize the Excel application
            Excel.Application excelApp = new Excel.Application();
            excelApp.DisplayAlerts = false;

            // Open the workbook
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);

            // Get the first worksheet
            Excel.Worksheet worksheet = workbook.Sheets[1]; // Index is 1-based

            // Get the used range of the worksheet
            Excel.Range usedRange = worksheet.UsedRange;
            int rowCount = usedRange.Rows.Count;

            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;

            try
            {
                foreach (string columnIndexStr in columnIndices)
                {
                    int columnIndex = int.Parse(columnIndexStr);

                    // Fill the cells in the column with zero, leaving the header intact
                    for (int rowIndex = 2; rowIndex <= rowCount; rowIndex++)
                    {
                        Excel.Range cell = (Excel.Range)worksheet.Cells[rowIndex, columnIndex];
                        cell.Value = 0;
                    }
                }

                workbook.SaveAs(csvFilePath);
            }

            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("An error occurred: " + ex.Message);
                CreateLogs("An error occurred: " + ex.Message, logFilePath);
                LogWithRed(ex.StackTrace);
                CreateLogs(ex.StackTrace, logFilePath);

            }

            finally
            {
                methodCount++;
                // Close the workbook and release resources
                workbook.Close();
                excelApp.Quit();

                // Release COM objects to avoid memory leaks
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

                // Optional: Force garbage collection to free up resources immediately
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
            }
        }

        public static void CopyColumnData(string filePath)
        {
            string[] argArr = args.Split(';');

            // Initialize the Excel application
            Excel.Application excelApp = new Excel.Application();
            excelApp.DisplayAlerts = false;

            // Open the workbook
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);

            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;

            // Get the first worksheet
            Excel.Worksheet worksheet = workbook.Sheets[1]; // Index is 1-based

            // Get the used range of the worksheet
            Excel.Range usedRange = worksheet.UsedRange;
            int rowCount = usedRange.Rows.Count;

            try
            {
                foreach (string pair in argArr)
                {
                    // Split each pair into source and destination column indices
                    string[] indices = pair.Split('#');
                    int sourceColumnIndex = int.Parse(indices[0]);
                    int destinationColumnIndex = int.Parse(indices[1]);

                    for (int rowIndex = 2; rowIndex <= rowCount; rowIndex++)
                    {
                        Excel.Range sourceCell = (Excel.Range)worksheet.Cells[rowIndex, sourceColumnIndex];
                        Excel.Range destinationCell = (Excel.Range)worksheet.Cells[rowIndex, destinationColumnIndex];

                        // Copy data from source cell to destination cell
                        destinationCell.Value = sourceCell.Value;
                    }
                }
                workbook.SaveAs(csvFilePath);
            }

            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("An error occurred: " + ex.Message);
                CreateLogs("An error occurred: " + ex.Message, logFilePath);
                LogWithRed(ex.StackTrace);
                CreateLogs(ex.StackTrace, logFilePath);

            }

            finally
            {
                methodCount++;
                // Close the workbook and release resources
                workbook.Close();
                excelApp.Quit();

                // Release COM objects to avoid memory leaks
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

                // Optional: Force garbage collection to free up resources immediately
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
            }
        }

        public static void CalculatePercentageDelta(string filePath)
        {
            string[] argArr = args.Split('#');
            int numeratorColumn = int.Parse(argArr[0]);
            int denominatorColumn = int.Parse(argArr[1]);
            int deltaColumn = int.Parse(argArr[2]);

            Excel.Application excelApp = new Excel.Application();

            excelApp.DisplayAlerts = false;

            // Open the workbook
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);

            // Get the first worksheet
            Excel.Worksheet worksheet = workbook.Sheets[1]; // Index is 1-based

            // Get the used range of the worksheet
            Excel.Range usedRange = worksheet.UsedRange;

            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;

            try
            {


                double sumNirvanaMarketValueBase = 0;

                for (int row = 1; row <= usedRange.Rows.Count; row++)
                {
                    //sumNirvanaMarketValueBase += (double)(worksheet.Cells[row, denominatorColumn] as Excel.Range).Value;

                    object cellValue = (worksheet.Cells[row, denominatorColumn] as Excel.Range).Value;
                    if (cellValue != null && double.TryParse(cellValue.ToString(), out double cellValueDouble))
                    {
                        sumNirvanaMarketValueBase += Math.Round(cellValueDouble, 2);
                    }
                }

                for (int row = 1; row <= usedRange.Rows.Count; row++)
                {
                    //double diffMarketValueBase = (double)(worksheet.Cells[row, numeratorColumn] as Excel.Range).Value;
                    //double percentageDelta = diffMarketValueBase / sumNirvanaMarketValueBase;

                    //// Set the calculated value back to the cell
                    //worksheet.Cells[row, deltaColumn] = percentageDelta;

                    object cellValue = (worksheet.Cells[row, numeratorColumn] as Excel.Range).Value;
                    if (cellValue != null && double.TryParse(cellValue.ToString(), out double diffMarketValueBase))
                    {
                        diffMarketValueBase = Math.Round(diffMarketValueBase, 2);
                        double percentageDelta = diffMarketValueBase / sumNirvanaMarketValueBase;

                        // Set the calculated value back to the cell
                        (worksheet.Cells[row, deltaColumn] as Excel.Range).Value = percentageDelta;
                    }
                }

                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("An error occurred: " + ex.Message);
                CreateLogs("An error occurred: " + ex.Message, logFilePath);
                LogWithRed(ex.StackTrace);
                CreateLogs(ex.StackTrace, logFilePath);

            }

            finally
            {
                methodCount++;
                // Close the workbook and release resources
                workbook.Close();
                excelApp.Quit();

                // Release COM objects to avoid memory leaks
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

                // Optional: Force garbage collection to free up resources immediately
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
            }
        }


        public static void FormatColumnsWithThousandSeparator(string filePath)
        {
            string modifiedFileFolder = "";
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = null;
            excelApp.DisplayAlerts = false;
            Excel.Worksheet worksheet = null;
            try
            {
                workbook = excelApp.Workbooks.Open(filePath);
            }
            catch (Exception ex)
            {
                LogWithRed("Error opening Excel file: " + ex.Message);
                CreateLogs("Error opening Excel file: " + ex.Message, logFilePath);
                return;
            }

            try
            {
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;

                worksheet = (Excel.Worksheet)workbook.Sheets[1];

                string columnNumbers = args.ToString();
                string[] columns = columnNumbers.Split('#');

                foreach (string col in columns)
                {
                    if (int.TryParse(col, out int columnNumber) && columnNumber > 0)
                    {
                        Excel.Range columnRange = worksheet.Columns[columnNumber];
                        columnRange.NumberFormat = "#,##0.00;-#,##0.00;0.00"; // Applying the 1000 separator format for both integers and decimal numbers
                    }
                }

                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close(false);
                excelApp.Quit();

                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        public static void RemoveColorsFromColumns(string filePath)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = null;
            excelApp.DisplayAlerts = false;
            try
            {
                workbook = excelApp.Workbooks.Open(filePath);
            }
            catch (Exception ex)
            {
                LogWithRed("Error opening file: " + ex.Message);
                CreateLogs("Error opening file: " + ex.Message, logFilePath);
                return;
            }

            try
            {
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string newFilePath = outputFolder + @"\" + fname + extension;

                foreach (Excel.Worksheet worksheet in workbook.Sheets)
                {
                    Excel.Range usedRange = worksheet.UsedRange;
                    foreach (Excel.Range column in usedRange.Columns)
                    {
                        column.Interior.ColorIndex = Excel.XlColorIndex.xlColorIndexNone;
                    }
                    if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                }

                workbook.SaveAs(newFilePath);

            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close(false);
                excelApp.Quit();

                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);

                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        public static void AlignColumnData(string filePath)
        {

            string arguments = args.ToString();
            string modifiedFileFolder = "";
            string alignment = arguments.Split('|')[0];
            arguments = arguments.Split('|')[1];
            string[] columns = arguments.Split('#');
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = null;
            excelApp.DisplayAlerts = false;
            try
            {
                workbook = excelApp.Workbooks.Open(filePath);
            }
            catch (Exception ex)
            {
                LogWithRed("Error opening Excel file: " + ex.Message);
                CreateLogs("Error opening Excel file: " + ex.Message, logFilePath);
                return;
            }

            try
            {
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (!string.IsNullOrEmpty(modifiedFileFolder))
                {
                    fname = modifiedFileFolder;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;

                Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1];

                foreach (string col in columns)
                {
                    if (int.TryParse(col, out int columnNumber) && columnNumber > 0)
                    {
                        Excel.Range columnRange = worksheet.Columns[columnNumber];
                        if (alignment.ToLower() == "left")
                        {
                            columnRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                        }
                        else if (alignment.ToLower() == "right")
                        {
                            columnRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                        }
                        else
                        {
                            Console.WriteLine("Invalid alignment specified.");
                            CreateLogs("Invalid alignment specified.", logFilePath);
                            return;
                        }
                    }
                }

                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close(false);
                excelApp.Quit();
            }

        }

        public static void MergeCsvFilesToExcel(string filePath)
        {
            string arguments = args.ToString();
            string password = arguments.Split('|')[0];
            arguments = arguments.Split('|')[1];
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = null;
            excelApp.DisplayAlerts = false;

            try
            {
                if (password == "")
                {
                    workbook = excelApp.Workbooks.Open(filePath);
                }
                else
                {
                    workbook = excelApp.Workbooks.Open(filePath, Password: password);
                }
            }
            catch (Exception ex)
            {
                LogWithRed("Error opening Excel file: " + ex.Message);
                CreateLogs("Error opening Excel file: " + ex.Message, logFilePath);
                return;
            }

            try
            {
                string fname = Path.GetFileNameWithoutExtension(filePath);

                string extension = Path.GetExtension(filePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;


                string[] argumentsArray = arguments.Split('#');
                string sheetNameToFind = argumentsArray[0];
                string toMergeFilePath = argumentsArray[1];

                string[] allFilePaths = Directory.GetFiles(inputFolder);
                string requiredPath = "";
                foreach (string pathToFind in allFilePaths)
                {
                    if (pathToFind.Contains(toMergeFilePath))
                    {
                        requiredPath = pathToFind;
                        break;
                    }
                }

                if (requiredPath == "")
                {
                    Console.WriteLine("File not found in input folder.");
                    CreateLogs("File not found in input folder.", logFilePath);
                    return;
                }

                Excel.Worksheet otherSheet = null;
                foreach (Excel.Worksheet worksheet in workbook.Worksheets)
                {
                    if (!worksheet.Name.Contains(sheetNameToFind))
                    {
                        otherSheet = worksheet;
                        break;
                    }
                }

                foreach (Excel.Worksheet worksheet in workbook.Worksheets)
                {
                    if (worksheet.Name.Contains(sheetNameToFind))
                    {
                        string fileExtension = Path.GetExtension(requiredPath).ToLower();
                        if (fileExtension == ".csv")
                        {
                            // Read and copy CSV data
                            string[] csvLines = File.ReadAllLines(requiredPath);
                            for (int rowIndex = 0; rowIndex < csvLines.Length; rowIndex++)
                            {
                                string[] rowValues = csvLines[rowIndex].Split(',');
                                for (int colIndex = 0; colIndex < rowValues.Length; colIndex++)
                                {
                                    worksheet.Cells[rowIndex + 1, colIndex + 1] = rowValues[colIndex];
                                }
                            }
                        }
                        else if (fileExtension == ".xls" || fileExtension == ".xlsx")
                        {
                            // Read and copy Excel data
                            Excel.Workbook sourceWorkbook = excelApp.Workbooks.Open(requiredPath);
                            Excel.Worksheet sourceWorksheet = sourceWorkbook.Sheets[1];

                            Excel.Range usedRange = sourceWorksheet.UsedRange;
                            Excel.Range destinationRange = worksheet.Cells[1, 1];
                            usedRange.Copy(destinationRange);
                            sourceWorkbook.Close(false);

                        }

                        excelApp.CutCopyMode = (Excel.XlCutCopyMode)0;

                        // Activate a different sheet, then return to the original sheet
                        if (otherSheet != null)
                        {
                            otherSheet.Activate();
                        }
                        worksheet.Activate();

                        // Select and activate a single cell (e.g., A1)
                        worksheet.Cells[1, 1].Select();
                        worksheet.Cells[1, 1].Activate();

                        break;
                    }
                }

                workbook.SaveAs(csvFilePath);




            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close(false);
                excelApp.Quit();
                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        public static void ConvertColValueToTwoDecimals(string filePath)
        {
            string columnNumbers = args.ToString();
            // string password = columnNumbers.Split('|')[0];
            string[] columns = columnNumbers.Split('#');

            string modifiedFileFolder = "";
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = null;
            excelApp.DisplayAlerts = false;
            Excel.Worksheet worksheet = null;
            try
            {

                workbook = excelApp.Workbooks.Open(filePath);
            }
            catch (Exception ex)
            {
                LogWithRed(ex.Message.ToString());
                return;
            }

            try
            {
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;

                worksheet = (Excel.Worksheet)workbook.Sheets[1];



                //int columnNumber = -1;
                //columnNumber= int.Parse(args);
                foreach (string col in columns)
                {
                    if (int.TryParse(col, out int columnNumber) && columnNumber > 0)
                    {
                        Excel.Range columnRange = worksheet.Columns[columnNumber];
                        columnRange.NumberFormat = "0.00";
                    }
                }
                // string modified_filePath = Path.Combine(outputFolder, Path.GetFileName(filePath));
                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close(false);
                excelApp.Quit();

                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }


        public static void CreateLogs(string logMessage, string logFilePath, int dateCheck = 1)
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
                        writer.WriteLine("");
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
                        writer.WriteLine("");
                    }
                }
            }
            catch (Exception ex)
            {
                //exitcode = 6;
                LogWithRed(ex.Message.ToString());
                //CreateExceptionReport(ex.StackTrace);
            }

        }

        static void SetCellValueInGridRange(string filePath)
        {

            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                // Split the args variable to get the parameters
                string[] arr = args.Split('#');
                string row1Str = arr[0];
                string col1Str = arr[1];
                string row2Str = arr[2];
                string col2Str = arr[3];
                object value = arr[4];
                value = ReplaceDateTimePlaceholders(value.ToString());
                // Create a new Excel application
                excelApp = new Excel.Application();

                excelApp.DisplayAlerts = false;

                // Open the workbook
                workbook = excelApp.Workbooks.Open(filePath);

                // Get the first worksheet
                worksheet = workbook.Sheets[1]; // Index is 1-based

                // Get the last row and column numbers

                int lastRow = worksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
                int lastColumn = worksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Column;


                // Parse the row and column values, or set to last row/column if null or empty
                int row1 = string.IsNullOrEmpty(row1Str) ? lastRow : int.Parse(row1Str);
                int col1 = string.IsNullOrEmpty(col1Str) ? lastColumn : int.Parse(col1Str);
                int row2 = string.IsNullOrEmpty(row2Str) ? lastRow : int.Parse(row2Str);
                int col2 = string.IsNullOrEmpty(col2Str) ? lastColumn : int.Parse(col2Str);

                // Set the value in the specified range of cells
                for (int row = row1; row <= row2; row++)
                {
                    for (int col = col1; col <= col2; col++)
                    {
                        worksheet.Cells[row, col] = value;
                    }
                }

                // Save the workbook
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (!string.IsNullOrEmpty(newfilename))
                {
                    fname = newfilename;
                }
                string csvFilePath = Path.Combine(outputFolder, fname + extension);

                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                // Handle the exception (e.g., log it)
                LogWithRed("An error occurred: " + ex.Message);
                CreateLogs("An error occurred: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                // Ensure the workbook and Excel application are closed properly
                if (workbook != null)
                {
                    workbook.Close(false);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                }
                if (excelApp != null)
                {
                    excelApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                }

                if (worksheet != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                }

                // Optional: Force garbage collection to free up resources immediately
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

      



        public static void LW_JPM_UpdateDataFromOtherFile(string FilePath)
        {


            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fname = Path.GetFileNameWithoutExtension(FilePath);
            string extension = Path.GetExtension(FilePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(FilePath);

            string mainFilePath = FilePath;
            string partialFilePath = currentDirectory;
            string[] arr = args.Split('#');
            if (!arr[0].Equals("NA"))
            {
                partialFilePath = currentDirectory + arr[0];
            }

            string partialFileName = arr[1];
            int startRow = int.Parse(arr[2]);
            string[] matchingFiles = FindFilesWithPartialName(partialFilePath, partialFileName);
            string otherFilePath = matchingFiles[0];

            // Ensure both files exist
            if (!File.Exists(mainFilePath))
            {
                Console.WriteLine("Main file does not exist.");
                CreateLogs("Main file does not exist.", logFilePath);
                return;
            }

            if (!File.Exists(otherFilePath))
            {
                Console.WriteLine("Other file does not exist.");
                CreateLogs("Other file does not exist.", logFilePath);
                return;
            }

            // Create Excel Application instance
            Excel.Application excelApp = new Excel.Application
            {
                ScreenUpdating = false
            };
            // Open main workbook
            Excel.Workbook mainWorkbook = excelApp.Workbooks.Open(mainFilePath);
            // Open other workbook
            Excel.Workbook otherWorkbook = excelApp.Workbooks.Open(otherFilePath);

            // Get main worksheet
            Excel.Worksheet mainSheet = mainWorkbook.Sheets[1]; // Assuming data is in the first worksheet

            // Get other worksheet
            Excel.Worksheet otherSheet = otherWorkbook.Sheets[1]; // Assuming data is in the first worksheet


            try
            {

                // Get last row in other worksheet
                int lastRow = otherSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
                double sum = 0;
                // Iterate through rows in other file starting from startRow
                for (int rowIndex = startRow; rowIndex <= lastRow; rowIndex++)
                {
                    // Check if the row is not empty and meets the conditions
                    if (otherSheet.Cells[rowIndex, 1].Value != null &&
                        otherSheet.Cells[rowIndex, 5].Value?.ToString() == "USD" &&
                        otherSheet.Cells[rowIndex, 2].Value?.ToString() == "Cash" &&
                        otherSheet.Cells[rowIndex, 3].Value?.ToString() == "Cash")
                    {
                        // Sum the values of the 6th and 7th columns
                        double value6 = otherSheet.Cells[rowIndex, 6].Value != null ? (double)otherSheet.Cells[rowIndex, 6].Value : 0;
                        double value7 = otherSheet.Cells[rowIndex, 7].Value != null ? (double)otherSheet.Cells[rowIndex, 7].Value : 0;
                        //for credit value we are taking 7th colun as negative
                        sum = value6 - value7;
                        break;


                    }
                }
                int mainLastRow = mainSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
                for (int mainRowIndex = 1; mainRowIndex <= mainLastRow; mainRowIndex++)
                {
                    if (mainSheet.Cells[mainRowIndex, 3].Value?.ToString() == "USD")
                    {
                        mainSheet.Cells[mainRowIndex, 4].Value = sum; // Set sum in the 4th column
                        break; // Assuming you only want to set the first matching row
                    }
                }
                // Save changes to main workbook
                mainWorkbook.SaveAs(csvFilePath);

                // Close workbooks
                mainWorkbook.Close();
                otherWorkbook.Close();

                // Close Excel Application
                excelApp.Quit();

                Console.WriteLine("Data appended to main file at: " + csvFilePath);
                CreateLogs("Data appended to main file at: " + csvFilePath, logFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("An error occurred: " + ex.Message);
                CreateLogs("An error occurred: " + ex.Message, logFilePath);

            }
            finally
            {
                methodCount++;
                // Release COM objects
                if (mainWorkbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(mainWorkbook);
                if (otherWorkbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(otherWorkbook);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        public static void FilterAndDeleteArgumentRowsList(string filePath)
        {
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(filePath);
            //outputFolder += "\\" + fileName;
            //string outputFolder = Path.Combine(Path.GetDirectoryName(filePath), "Output_Files");
            //string outputFile = Path.Combine(outputFolder, fileName);

            // Load Excel application
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[1]; // Assuming the first sheet

            excelApp.DisplayAlerts = false;
            int columnIndex;
            string searchText;
            string[] searchTextArr;
            List<string> searchTextList;
            string headerText;
            int startRowIndex;
            if (SplitParameter != "")
            {
                char[] charArray = SplitParameter.ToCharArray();
                string[] arr2 = args.Split(charArray[0]);
                columnIndex = int.Parse(arr2[0]);
                searchText = arr2[1];
                searchTextArr = searchText.Split(';');
                searchTextList = searchTextArr.ToList();
                headerText = arr2[2];
                startRowIndex = int.Parse(arr2[3]);


            }
            else
            {
                string[] arr = args.Split('#');
                columnIndex = int.Parse(arr[0]);
                searchText = arr[1];
                searchTextArr = searchText.Split(';');
                searchTextList = searchTextArr.ToList();
                headerText = arr[2];
                startRowIndex = int.Parse(arr[3]);

                // Filter and delete rows

            }
            Excel.Range usedRange = sheet.UsedRange;
            int rowCount = usedRange.Rows.Count;
            int lastRow = usedRange.Rows.Count;
            // Create a list to hold the rows to delete
            // Find the next available row for pasting (after the header)
            int pasteRow = startRowIndex; // Start pasting from the second row


            try
            {

                for (int rowIndex = startRowIndex; rowIndex <= rowCount; rowIndex++)
                {
                    Excel.Range cell = (Excel.Range)sheet.Cells[rowIndex, columnIndex];
                    string cellContent = cell.Value != null ? cell.Value.ToString() : "";
                    cellContent = cellContent.Trim();
                    if (!(SearchTextContainsItems(searchTextList, cellContent) && !cellContent.Equals(headerText)))
                    {
                        //Excel.Range rowToDelete = (Excel.Range)sheet.Rows[rowIndex];
                        //rowToDelete.Delete();
                        //rowIndex--; // Adjust rowIndex since the next row moves up after deletion
                        //rowCount--; // Adjust rowCount after deletion
                        // Copy entire row to the new location
                        Excel.Range sourceRow = (Excel.Range)sheet.Rows[rowIndex];
                        Excel.Range destinationRow = (Excel.Range)sheet.Rows[pasteRow];
                        sourceRow.Copy(destinationRow);

                        // Increment pasteRow for the next row to paste
                        pasteRow++;
                    }
                }
                // Clear contents of rows below the last pasted row
                if (pasteRow <= lastRow)
                {
                    Excel.Range clearRange = sheet.Rows[pasteRow].Resize[lastRow - pasteRow + 1];
                    clearRange.ClearContents();
                }

                // Save changes
                workbook.SaveAs(csvFilePath);
                Console.WriteLine("Rows not containing the specified text in the specified column have been deleted.");
                CreateLogs("Rows not containing the specified text in the specified column have been deleted.", logFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();


                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (sheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }


        }

        public static void FilterAndPerformSubstraction(string filePath)
        {


            int filterColumnNumber;
            string targetValue;
            int sourceColumnNumber1;
            int sourceColumnNumber2;

            int targetColumnNumber;
            int startFilterRow;




            string[] arr = args.Split('#');
            int columnIndex = int.Parse(arr[0]);
            string searchText = arr[1];
            string[] searchTextArr = searchText.Split(';');
            List<string> searchTextList = searchTextArr.ToList();
            string headerText = arr[2];
            int startRowIndex = int.Parse(arr[3]);



            sourceColumnNumber1 = int.Parse(arr[4]);
            sourceColumnNumber2 = int.Parse(arr[5]);
            targetColumnNumber = int.Parse(arr[6]);


            ///////////////////////////////////////////////////////////////////
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(filePath);
            //outputFolder += "\\" + fileName;
            //string outputFolder = Path.Combine(Path.GetDirectoryName(filePath), "Output_Files");
            //string outputFile = Path.Combine(outputFolder, fileName);

            // Load Excel application
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[1]; // Assuming the first sheet

            excelApp.DisplayAlerts = false;
            // Extract filter criteria

            // Filter and delete rows
            Excel.Range usedRange = sheet.UsedRange;
            int rowCount = usedRange.Rows.Count;

            try
            {

                for (int rowIndex = startRowIndex; rowIndex <= rowCount; rowIndex++)
                {
                    Excel.Range cell = (Excel.Range)sheet.Cells[rowIndex, columnIndex];
                    string cellContent = cell.Value != null ? cell.Value.ToString() : "";
                    cellContent = cellContent.Trim();
                    if ((searchTextList.Contains(cellContent) && !cellContent.Equals(headerText)))
                    {
                        Excel.Range cell1 = (Excel.Range)sheet.Cells[rowIndex, sourceColumnNumber1];
                        Excel.Range cell2 = (Excel.Range)sheet.Cells[rowIndex, sourceColumnNumber2];
                        Excel.Range resultCell = (Excel.Range)sheet.Cells[rowIndex, targetColumnNumber];
                        double value1;
                        double value2;
                        Console.WriteLine(cell1.Value);
                        if (cell1.Value != null && cell2.Value != null)
                        {
                            value1 = Convert.ToDouble(cell1.Value);
                            value2 = Convert.ToDouble(cell2.Value);
                            double result = value1 - value2;
                            resultCell.Value = result;
                        }
                        else
                        {
                            resultCell.Value = "";
                        }
                    }
                }

                // Save changes
                workbook.SaveAs(csvFilePath);
                Console.WriteLine("Rows not containing the specified text in the specified column have been deleted.");
                CreateLogs("Rows not containing the specified text in the specified column have been deleted.", logFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();


                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (sheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }




        }

        public static void FilterAndPerformMultiplication(string filePath)
        {
            int filterColumnNumber;
            string targetValue;
            int sourceColumnNumber1;
            int sourceColumnNumber2;
            int targetColumnNumber;
            int startFilterRow;


            string[] arr = args.Split('#');
            int columnIndex = int.Parse(arr[0]);
            string searchText = arr[1];
            string[] searchTextArr = searchText.Split(';');
            List<string> searchTextList = searchTextArr.ToList();
            string headerText = arr[2];
            int startRowIndex = int.Parse(arr[3]);



            sourceColumnNumber1 = int.Parse(arr[4]);
            sourceColumnNumber2 = int.Parse(arr[5]);
            targetColumnNumber = int.Parse(arr[6]);


            ///////////////////////////////////////////////////////////////////
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(filePath);
            //outputFolder += "\\" + fileName;
            //string outputFolder = Path.Combine(Path.GetDirectoryName(filePath), "Output_Files");
            //string outputFile = Path.Combine(outputFolder, fileName);

            // Load Excel application
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[1]; // Assuming the first sheet

            excelApp.DisplayAlerts = false;
            // Extract filter criteria
            try
            {
                // Filter and delete rows
                Excel.Range usedRange = sheet.UsedRange;
                int rowCount = usedRange.Rows.Count;


                for (int rowIndex = startRowIndex; rowIndex <= rowCount; rowIndex++)
                {
                    Excel.Range cell = (Excel.Range)sheet.Cells[rowIndex, columnIndex];
                    string cellContent = cell.Value != null ? cell.Value.ToString() : "";
                    cellContent = cellContent.Trim();
                    if ((searchTextList.Contains(cellContent) && !cellContent.Equals(headerText)))
                    {
                        Excel.Range cell1 = (Excel.Range)sheet.Cells[rowIndex, sourceColumnNumber1];
                        Excel.Range cell2 = (Excel.Range)sheet.Cells[rowIndex, sourceColumnNumber2];
                        Excel.Range resultCell = (Excel.Range)sheet.Cells[rowIndex, targetColumnNumber];
                        double value1;
                        double value2;

                        if (cell1.Value != null && cell2.Value != null)
                        {
                            value1 = Convert.ToDouble(cell1.Value);
                            value2 = Convert.ToDouble(cell2.Value);
                            double result = value1 * value2;
                            resultCell.Value = result;
                        }
                        else
                        {
                            resultCell.Value = "";
                        }
                    }
                }
                // Save changes
                workbook.SaveAs(csvFilePath);
            }

            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);

            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();


                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (sheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }

        }

        public static void FilterAndDeleteRowsBelowDate(string filePath)
        {
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(filePath);
            //outputFolder += "\\" + fileName;
            //string outputFolder = Path.Combine(Path.GetDirectoryName(filePath), "Output_Files");
            //string outputFile = Path.Combine(outputFolder, fileName);

            // Load Excel application
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[1]; // Assuming the first sheet
            try
            {
                excelApp.DisplayAlerts = false;
                // Extract filter criteria
                string[] arr = args.Split('#');
                int columnIndex = int.Parse(arr[0]);
                string dateFilePath = arr[1];
                string headerText = arr[2];
                int startRowIndex = int.Parse(arr[3]);

                // Read date from the text file
                DateTime specifiedDate;
                using (StreamReader reader = new StreamReader(dateFilePath))
                {
                    string dateString = reader.ReadLine();
                    specifiedDate = DateTime.ParseExact(dateString, "MM/dd/yyyy", null);
                }
                // Filter and delete rows
                Excel.Range usedRange = sheet.UsedRange;
                int rowCount = usedRange.Rows.Count;
                DateTime maxDate = specifiedDate;


                for (int rowIndex = startRowIndex; rowIndex <= rowCount; rowIndex++)
                {
                    Excel.Range cell = (Excel.Range)sheet.Cells[rowIndex, columnIndex];
                    string cellContent = cell.Value != null ? cell.Value.ToString() : "";
                    cellContent = cellContent.Trim();

                    if (DateTime.TryParse(cellContent, out DateTime cellDate))
                    {
                        if (cellDate <= specifiedDate && !cellContent.Equals(headerText))
                        {
                            Excel.Range rowToDelete = (Excel.Range)sheet.Rows[rowIndex];
                            rowToDelete.Delete();
                            rowIndex--; // Adjust rowIndex since the next row moves up after deletion
                            rowCount--; // Adjust rowCount after deletion
                        }
                        else if (cellDate > maxDate)
                        {
                            maxDate = cellDate;
                        }
                    }
                }

                workbook.SaveAs(csvFilePath);
                // Update the date file with the maximum date
                using (StreamWriter writer = new StreamWriter(dateFilePath))
                {
                    writer.WriteLine(maxDate.ToString("MM/dd/yyyy"));
                }
            }

            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();


                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }


            Console.WriteLine("Rows with dates below or equal to the specified date have been deleted.");
            CreateLogs("Rows with dates below or equal to the specified date have been deleted.", logFilePath);
        }
      
        public static void ChangeColumnsToDateNew(string filePath)
        {
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string[] arr = args.Split('#');
            List<string> arguments = new List<string>(arr);
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet worksheet = workbook.ActiveSheet;
            excelApp.DisplayAlerts = false;

            string[] columnNames;
            string columnNamesList = arr[0];
            columnNames = columnNamesList.Split(';');
            int startRowIndex = int.Parse(arr[1]);
            string dateFormat = arr[2];


            Excel.Range usedRange = worksheet.UsedRange;

            try
            {

                Excel.Range targetColumn = null;

                // Find the target column by searching for the column name in each row
                foreach (string columnName in columnNames)
                {
                    for (int columnIndex = 1; columnIndex <= usedRange.Columns.Count; columnIndex++)
                    {
                        Excel.Range cell = (Excel.Range)usedRange.Cells[startRowIndex, columnIndex];
                        if (cell.Value2 != null && cell.Value2.ToString() == columnName)
                        {
                            targetColumn = worksheet.Range[cell, worksheet.Cells[worksheet.Rows.Count, cell.Column]];
                            break;
                        }
                        // Your logic here
                    }


                    if (targetColumn != null)
                    {
                        // Change the format of the target column to mm/dd/yyyy
                        targetColumn.NumberFormat = dateFormat;
                    }
                    else
                    {
                        Console.WriteLine($"Column '{columnName}' not found.");
                        CreateLogs($"Column '{columnName}' not found.", logFilePath);
                    }
                }


                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();
                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }
        static void ConvertexttoColInMemoryCSVFiles(string excelFilePath)
        {
            string csvFilePath = "";
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;
            string[] arr = args.Split('#');
            string delimiter = arr[0];
            try
            {
                string fname = newfilename != "" ? newfilename : Path.GetFileNameWithoutExtension(excelFilePath);
                csvFilePath = Path.Combine(outputFolder, $"{fname}.csv");

                using (StreamReader sr = new StreamReader(excelFilePath))
                using (StreamWriter sw = new StreamWriter(csvFilePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string updatedLine = line.Replace(delimiter, ","); // Replace delimiter with comma
                        sw.WriteLine(updatedLine);
                    }
                }


                CreateLogs($"Conversion text to column completed for {fname}", logFilePath);
                Console.WriteLine($"Conversion text to column completed for {fname}");
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed($"Error: {ex.Message}");
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;

            }
        }
        public static void ChangeColumnsToDateNew_V2(string filePath)
        {
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string[] arr = args.Split('#');
            List<string> arguments = new List<string>(arr);
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet worksheet = workbook.ActiveSheet;
            excelApp.DisplayAlerts = false;

            string[] columnNames;
            string columnNamesList = arr[0];
            // Normalize to replace `\r\n` with `\n` properly
            columnNamesList = columnNamesList.Replace("\\r\\n", "\n").Replace("\r\n", "\n");
            columnNames = columnNamesList.Split(';');
            int startRowIndex = int.Parse(arr[1]);
            string dateFormat = arr[2];


            Excel.Range usedRange = worksheet.UsedRange;

            try
            {

                Excel.Range targetColumn = null;

                // Find the target column by searching for the column name in each row
                foreach (string columnName in columnNames)
                {
                    string normalizedColumnName = columnName.Replace("\\n", "\n").Replace("\\r", "").Trim();
                    for (int columnIndex = 1; columnIndex <= usedRange.Columns.Count; columnIndex++)
                    {
                        Excel.Range cell = (Excel.Range)usedRange.Cells[startRowIndex, columnIndex];

                        if (cell.Value2 != null)
                        {
                            // Normalize Excel cell value by replacing `\r\n` with `\n`
                            string cellValue = cell.Value2.ToString().Replace("\r\n", "\n").Trim();

                            if (cellValue == columnName.Trim())
                            {
                                targetColumn = worksheet.Range[cell, worksheet.Cells[worksheet.Rows.Count, cell.Column]];
                                break;
                            }
                        }
                        // Your logic here
                    }


                    if (targetColumn != null)
                    {
                        // Change the format of the target column to mm/dd/yyyy
                        targetColumn.NumberFormat = dateFormat;
                    }
                    else
                    {
                        Console.WriteLine($"Column '{columnName}' not found.");
                        CreateLogs($"Column '{columnName}' not found.", logFilePath);
                    }
                }


                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();
                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        public static void CopyAndModifyValues(string sourceFilePath)
        {
            // Create an instance of Excel application
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook sourceWorkbook = null;
            Excel.Workbook targetWorkbook = null;
            Excel.Worksheet sourceWorksheet = null;
            Excel.Worksheet targetWorksheet = null;
            string[] arr = args.Split('#');

            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string targetFilePath = currentDirectory + "\\" + arr[0];
            int sourceColumnToCheck = int.Parse(arr[1]);
            int sourceQuantityColumn = int.Parse(arr[2]);
            int sourceSymbolColumn = int.Parse(arr[3]);
            int targetSymbolColumn = int.Parse(arr[4]);
            int targetQuantityColumn = int.Parse(arr[5]);
            int targetPasteColumn = int.Parse(arr[6]);
            int sourceFileStartRowOfSearching = int.Parse(arr[7]);
            int targetFileStartRowOfSearching = int.Parse(arr[8]);
            try
            {
                // Open source and target workbooks
                sourceWorkbook = excelApp.Workbooks.Open(sourceFilePath);
                targetWorkbook = excelApp.Workbooks.Open(targetFilePath);

                // Assume data is in the first worksheet
                sourceWorksheet = sourceWorkbook.Sheets[1];
                targetWorksheet = targetWorkbook.Sheets[1];

                // Get the used range of the source worksheet
                Excel.Range sourceRange = sourceWorksheet.UsedRange;

                for (int row = sourceFileStartRowOfSearching; row <= sourceRange.Rows.Count; row++)
                {
                    // Get the value from the column to check
                    double valueToCheck = Convert.ToDouble((sourceRange.Cells[row, sourceColumnToCheck] as Excel.Range).Value2);
                    if (valueToCheck > 0 && valueToCheck < 1000)
                    {
                        // Get the corresponding quantity and symbol from source
                        double sourceQuantity = Convert.ToDouble((sourceRange.Cells[row, sourceQuantityColumn] as Excel.Range).Value2);
                        string sourceSymbol = (sourceRange.Cells[row, sourceSymbolColumn] as Excel.Range).Value2.ToString();

                        // Find the corresponding row in the target worksheet
                        Excel.Range targetRange = targetWorksheet.UsedRange;
                        for (int targetRow = targetFileStartRowOfSearching; targetRow <= targetRange.Rows.Count; targetRow++)
                        {
                            string targetSymbol = (targetRange.Cells[targetRow, targetSymbolColumn] as Excel.Range).Value2.ToString();
                            double targetQuantity = Convert.ToDouble((targetRange.Cells[targetRow, targetQuantityColumn] as Excel.Range).Value2);

                            if (targetSymbol.Contains(sourceSymbol) && Math.Abs(sourceQuantity) == Math.Abs(targetQuantity))
                            {
                                // Paste the value to the target column
                                targetWorksheet.Cells[targetRow, targetPasteColumn] = valueToCheck;
                            }
                        }
                        if (targetRange != null)
                        {
                            Marshal.FinalReleaseComObject(targetRange);
                        }
                    }
                }

                if (sourceRange != null)
                {
                    Marshal.FinalReleaseComObject(sourceRange);
                }

                string fname = Path.GetFileNameWithoutExtension(targetFilePath);
                string extension = Path.GetExtension(targetFilePath);


                string csvFilePath = Path.Combine(outputFolder, fname + extension);

                // Save the target workbook
                targetWorkbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("An error occurred: " + ex.Message);
                CreateLogs("An error occurred: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;

                if (sourceWorkbook != null)
                {
                    sourceWorkbook.Close(false);
                    Marshal.FinalReleaseComObject(sourceWorkbook);
                }
                if (sourceWorksheet != null)
                {
                    Marshal.FinalReleaseComObject(sourceWorksheet);
                }

                if (targetWorkbook != null)
                {
                    targetWorkbook.Close(false);
                    Marshal.FinalReleaseComObject(targetWorkbook);
                }
                if (targetWorksheet != null)
                {
                    Marshal.FinalReleaseComObject(targetWorksheet);
                }

                excelApp.Quit();
                Marshal.FinalReleaseComObject(excelApp);
            }
        }


        public static void DeleteRowsExceptHeaderAndLatestDate(string filePath)
        {
            string[] arr = args.Split('#');
            int dateColumnIndex = int.Parse(arr[0]);
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {


                excelApp = new Excel.Application();
                workbook = excelApp.Workbooks.Open(filePath);

                worksheet = workbook.Sheets[1];

                Excel.Range usedRange = worksheet.UsedRange;
                int rowCount = usedRange.Rows.Count;

                if (rowCount <= 1)
                {
                    return; // Nothing to delete
                }

                DateTime latestDate = DateTime.MinValue;

                // Find the latest date in the specified column
                for (int row = 2; row <= rowCount; row++)
                {
                    string temp = worksheet.Cells[row, dateColumnIndex].Value.ToString();
                    if (DateTime.TryParse(worksheet.Cells[row, dateColumnIndex].Value.ToString(), out DateTime date))
                    {
                        // Consider only the date part for comparison
                        date = date.Date;
                        if (date > latestDate)
                        {
                            latestDate = date;
                        }
                    }
                }

                // Delete rows that do not contain the latest date
                for (int row = rowCount; row >= 2; row--)
                {
                    if (DateTime.TryParse(worksheet.Cells[row, dateColumnIndex].Value.ToString(), out DateTime date))
                    {
                        if (date != latestDate)
                        {
                            ((Excel.Range)worksheet.Rows[row]).Delete();
                        }
                    }
                }

                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (!string.IsNullOrEmpty(newfilename))
                {
                    fname = newfilename;
                }

                string csvFilePath = Path.Combine(outputFolder, fname + extension);

                // Save and close the workbook
                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("An error occurred: " + ex.Message);
                CreateLogs("An error occurred: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                // Release resources
                if (workbook != null)
                {
                    workbook.Close(false);
                    Marshal.ReleaseComObject(workbook);
                }
                if (excelApp != null)
                {
                    excelApp.Quit();
                    Marshal.ReleaseComObject(excelApp);
                }
            }
        }

        public static void DeleteRowsBasedOnSearchText(string filePath)
        {
            try
            {
                string[] arr = args.Split('#');
                int columnIndex = int.Parse(arr[0]);
                string searchText = arr[1];
                int headerRowIndex = 1;
                if (arr.Length > 2)
                {
                    headerRowIndex = int.Parse(arr[2]);
                }

                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("The specified file does not exist.");
                }

                // Read all lines from the CSV file
                var lines = File.ReadAllLines(filePath).ToList();
                if (lines.Count == 0)
                {
                    throw new InvalidOperationException("The CSV file is empty.");
                }

                // Split the first line to determine the number of columns (header)

                var header = lines[headerRowIndex - 1].Split(',');



                // Check if the column index is valid
                if (columnIndex < 0 || columnIndex >= header.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(columnIndex), "Column index is out of range.");
                }

                // Filter out rows that match the search text in the specified column
                var filteredLines = new List<string> { lines[headerRowIndex - 1] }; // Keep the header
                for (int i = headerRowIndex; i < lines.Count; i++)
                {
                    var columns = lines[i].Split(',');
                    try
                    {
                        if (columns[columnIndex - 1].Equals(searchText))
                        {
                            filteredLines.Add(lines[i]);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogWithRed(ex.Message.ToString());
                        continue;
                    }

                }




                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = ".csv";
                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;

                // Write the filtered lines back to the CSV file
                File.WriteAllLines(csvFilePath, filteredLines);
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
        }

        public static void DeleteRowsBasedOnSearchTextList(string filePath)
        {
            try
            {
                string[] arr = args.Split('#');
                int columnIndex = int.Parse(arr[0]);
                string searchText = arr[1];
                string[] searchTexts = searchText.Split(';');

                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("The specified file does not exist.");
                }

                // Read all lines from the CSV file
                var lines = File.ReadAllLines(filePath).ToList();
                if (lines.Count == 0)
                {
                    throw new InvalidOperationException("The CSV file is empty.");
                }

                // Split the first line to determine the number of columns (header)
                var header = lines[0].Split(',');

                // Check if the column index is valid
                if (columnIndex < 0 || columnIndex >= header.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(columnIndex), "Column index is out of range.");
                }

                // Filter out rows that match the search text in the specified column
                var filteredLines = new List<string> { lines[0] }; // Keep the header
                for (int i = 1; i < lines.Count; i++)
                {
                    var columns = lines[i].Split(',');
                    if (searchTexts.Contains(columns[columnIndex - 1]))
                    {
                        filteredLines.Add(lines[i]);
                    }
                }




                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = ".csv";
                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;

                // Write the filtered lines back to the CSV file
                File.WriteAllLines(csvFilePath, filteredLines);
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
        }

        public static void DeleteRowsBasedOnSearchTextListIfContains(string filePath)
        {
            try
            {
                string[] arr = args.Split('#');
                int columnIndex = int.Parse(arr[0]);
                string searchText = arr[1];
                string[] searchTexts = searchText.Split(';');

                // Check if the file exists
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("The specified file does not exist.");
                }

                // Read all lines from the CSV file
                var lines = File.ReadAllLines(filePath).ToList();
                if (lines.Count == 0)
                {
                    throw new InvalidOperationException("The CSV file is empty.");
                }

                // Get the header and check column index validity
                var header = lines[0].Split(',');
                if (columnIndex <= 0 || columnIndex > header.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(columnIndex), "Column index is out of range.");
                }

                // Adjust columnIndex to 0-based for array indexing
                columnIndex--;

                // Filter out rows where the specified column contains any of the search texts
                var filteredLines = new List<string> { lines[0] }; // Keep the header
                for (int i = 1; i < lines.Count; i++)
                {
                    var columns = lines[i].Split(',');
                    if (searchTexts.Any(text => columns[columnIndex].Contains(text)))
                    {
                        filteredLines.Add(lines[i]);
                    }
                }

                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = ".csv";
                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;

                // Write the filtered lines to the new CSV file
                File.WriteAllLines(csvFilePath, filteredLines);
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
        }


        public static void ConvertColumnToNumberInCSV(string filePath)
        {
            string[] arr = args.Split('#');
            int columnNumber = int.Parse(arr[0]);
            int decimalPlaces = int.Parse(arr[1]);
            // string tempFilePath = Path.GetTempFileName();
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = ".csv";
            if (!string.IsNullOrEmpty(newfilename))
            {
                fname = newfilename;
            }

            string csvFilePath = Path.Combine(outputFolder, fname + extension);
            try
            {
                using (var reader = new StreamReader(filePath))
                using (var writer = new StreamWriter(csvFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] values = line.Split(',');

                        if (columnNumber - 1 < values.Length)
                        {
                            // Try to convert the cell value to a double and skip if it's not convertible
                            double numericValue;
                            if (double.TryParse(values[columnNumber - 1], NumberStyles.Any, CultureInfo.InvariantCulture, out numericValue))
                            {
                                // Format the value to the specific number of decimal places
                                values[columnNumber - 1] = numericValue.ToString("F" + decimalPlaces, CultureInfo.InvariantCulture);
                            }
                            // If conversion fails, the original value remains unchanged
                        }

                        // Write the modified line back to the temporary file
                        writer.WriteLine(string.Join(",", values));
                    }
                }
                // Replace original file with the updated file
                File.Copy(csvFilePath, filePath, true);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
            }
            finally
            {
                methodCount++;
                // Clean up the temporary file
                if (File.Exists(csvFilePath))
                {
                    File.Delete(csvFilePath);
                }
            }
        }

        public static void ConvertColValueUptoParticularDecimals(string filePath)
        {

            string[] arr = args.Split('#');
            int columnNumber = int.Parse(arr[0]);
            int decimalPlaces = int.Parse(arr[1]);

            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;
            excelApp.DisplayAlerts = false;
            try
            {

                workbook = excelApp.Workbooks.Open(filePath);
            }
            catch (Exception ex)
            {
                LogWithRed(ex.Message.ToString());
                return;
            }
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = ".csv";
            if (!string.IsNullOrEmpty(newfilename))
            {
                fname = newfilename;
            }

            string csvFilePath = Path.Combine(outputFolder, fname + extension);

            try
            {
                // Open the workbook
                workbook = excelApp.Workbooks.Open(filePath);
                worksheet = (Excel.Worksheet)workbook.Sheets[1]; // Assuming the first sheet

                // Get the used range in the worksheet
                Excel.Range usedRange = worksheet.UsedRange;

                // Loop through each row in the specified column
                for (int row = 1; row <= usedRange.Rows.Count; row++)
                {
                    Excel.Range cell = (Excel.Range)worksheet.Cells[row, columnNumber];

                    if (cell.Value2 != null)
                    {
                        try
                        {
                            // Convert the cell value to a double (numeric value)
                            double numericValue = Convert.ToDouble(cell.Value2);

                            // Round the value to the specified decimal places
                            numericValue = Math.Round(numericValue, decimalPlaces);

                            // Set the value back in the cell and apply number format
                            cell.Value2 = numericValue;

                            // Set the number format to display the correct number of decimal places
                            string numberFormat = "0." + new string('0', decimalPlaces);
                            cell.NumberFormat = numberFormat; // Set format like 0.00 for 2 decimal places
                        }
                        catch (FormatException)
                        {
                            // If the cell cannot be converted to a number, ignore it or handle it
                            LogWithRed($"Cell {row}, {columnNumber} is not a valid number.");
                        }
                    }
                }
                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
            }

            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();


                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
            }
        }

        public static void ConvertAllDatToTxt(string filePath)
        {
            try
            {
                // Check if the directory exists
                if (!Directory.Exists(inputFolder))
                {
                    Console.WriteLine("The specified directory does not exist.");
                    return;
                }

                // Get all .DAT files in the directory
                string[] datFiles = Directory.GetFiles(inputFolder, "*.dat");

                // Iterate through each .DAT file
                foreach (string datFilePath in datFiles)
                {
                    // Generate the corresponding .TXT file path
                    string txtFilePath = Path.ChangeExtension(datFilePath, ".txt");

                    // Read all bytes from the .DAT file
                    byte[] datFileContents = File.ReadAllBytes(datFilePath);

                    // Write the bytes to the .TXT file
                    File.WriteAllBytes(txtFilePath, datFileContents);

                    Console.WriteLine($"Converted {Path.GetFileName(datFilePath)} to {Path.GetFileName(txtFilePath)}");
                    CreateLogs($"Converted {Path.GetFileName(datFilePath)} to {Path.GetFileName(txtFilePath)}", logFilePath);
                }

                Console.WriteLine("All .DAT files have been converted to .TXT files.");
                CreateLogs("All .DAT files have been converted to .TXT files.", logFilePath);
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed($"An error occurred: {ex.Message}");
                CreateLogs($"An error occurred: {ex.Message}", logFilePath);
            }
        }

        public static void DeleteFilesWithExtension(string directoryPath)
        {
            try
            {
                string[] arr = args.Split('#');
                string extension = arr[0];
                // Check if directory exists
                if (!Directory.Exists(inputFolder))
                {
                    Console.WriteLine("Directory does not exist.");
                    CreateLogs("Directory does not exist.", logFilePath);
                    return;
                }

                // Ensure the extension starts with a dot
                if (!extension.StartsWith("."))
                {
                    extension = "." + extension;
                }

                // Get all files with the specified extension in the directory
                string[] files = Directory.GetFiles(inputFolder, "*" + extension);

                // Delete each file
                foreach (string file in files)
                {
                    File.Delete(file);
                }
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed($"An error occurred: {ex.Message}");
                CreateLogs($"An error occurred: {ex.Message}", logFilePath);
            }
        }

        public static void ConvertTxtContainingBackTicksToCsvFast(string inputFilePath)
        {
            try
            {
                string fname = Path.GetFileNameWithoutExtension(inputFilePath);
                string extension = ".csv";
                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;
                using (StreamReader reader = new StreamReader(inputFilePath))
                using (StreamWriter writer = new StreamWriter(csvFilePath))
                {
                    string line;
                    // Read each line and replace ` with ,
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Replace backtick with comma and write to output
                        writer.WriteLine(line.Replace('`', ','));
                    }
                }

                Console.WriteLine("File conversion completed successfully.");
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
        }

        public static void ConvertTxtContainingBackTicksToCsv(string inputFilePath)
        {
            try
            {


                // Read all lines from the input file
                string[] lines = File.ReadAllLines(inputFilePath);

                // Replace backticks with commas in each line
                for (int i = 0; i < lines.Length; i++)
                {
                    lines[i] = lines[i].Replace('`', ',');
                }

                string fname = Path.GetFileNameWithoutExtension(inputFilePath);
                string extension = ".csv";
                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;
                // Write the modified lines to the output CSV file
                File.WriteAllLines(csvFilePath, lines);

                Console.WriteLine("File conversion successful.");
                CreateLogs("File conversion successful.", logFilePath);
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed($"An error occurred: {ex.Message}");
                CreateLogs($"An error occurred: {ex.Message}", logFilePath);
            }
        }

        public static void DivideColumnValues(string filePath)
        {
            string[] arr = args.Split('#');
            int columnIndex = int.Parse(arr[0]);
            double divisor = double.Parse(arr[1]);
            if (divisor == 0)
            {
                throw new ArgumentException("Divisor cannot be zero.");
            }

            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = null;
            excelApp.DisplayAlerts = false;
            try
            {
                // Open the workbook
                workbook = excelApp.Workbooks.Open(filePath);
                Excel.Worksheet worksheet = workbook.Sheets[1];

                // Find the last row in the specified column
                Excel.Range lastCell = worksheet.Cells[worksheet.Rows.Count, columnIndex].End(Excel.XlDirection.xlUp);
                int lastRow = lastCell.Row;

                // Loop through each cell in the column and divide its value by the divisor
                for (int row = 1; row <= lastRow; row++)
                {
                    Excel.Range cell = worksheet.Cells[row, columnIndex];
                    double cellValue = 0; // Initialize cellValue
                    if (cell.Value2 != null && double.TryParse(cell.Value2.ToString(), out cellValue))
                    {
                        cell.Value2 = cellValue / divisor;
                    }
                }
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;
                // Save the workbook
                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
            finally
            {
                methodCount++;
                // Close the workbook and release resources
                workbook?.Close(false);
                excelApp.Quit();

                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }


        public static void AppendAllColumns(string firstFilePath)
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string secondFilePath = currentDirectory;
            string[] arr = args.Split('#');
            if (!arr[0].Equals("NA"))
            {
                secondFilePath = currentDirectory + arr[0];
            }

            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook firstWorkbook = null;
            Excel.Workbook secondWorkbook = null;

            try
            {
                // Open the first workbook
                firstWorkbook = excelApp.Workbooks.Open(firstFilePath);
                Excel.Worksheet firstWorksheet = firstWorkbook.Sheets[1];

                // Open the second workbook
                secondWorkbook = excelApp.Workbooks.Open(secondFilePath);
                Excel.Worksheet secondWorksheet = secondWorkbook.Sheets[1];

                // Find the last row in the first workbook
                Excel.Range firstLastCell = firstWorksheet.Cells[firstWorksheet.Rows.Count, 1].End(Excel.XlDirection.xlUp);
                int firstLastRow = firstLastCell.Row;

                // Find the last row and last column in the second workbook
                Excel.Range secondLastRowCell = secondWorksheet.Cells[secondWorksheet.Rows.Count, 1].End(Excel.XlDirection.xlUp);
                int secondLastRow = secondLastRowCell.Row;
                Excel.Range secondLastColCell = secondWorksheet.Cells[1, secondWorksheet.Columns.Count].End(Excel.XlDirection.xlToLeft);
                int secondLastCol = secondLastColCell.Column;

                // Copy all columns from the second workbook to the first workbook
                for (int col = 1; col <= secondLastCol; col++)
                {
                    Excel.Range sourceRange = secondWorksheet.Range[secondWorksheet.Cells[1, col], secondWorksheet.Cells[secondLastRow, col]];

                    // Get the last used column in the first worksheet
                    int lastUsedCol = firstWorksheet.UsedRange.Columns.Count;

                    Excel.Range targetRange = firstWorksheet.Cells[1, lastUsedCol + 1];

                    // Resize the target range to match the source range and copy the values
                    targetRange.Resize[sourceRange.Rows.Count, sourceRange.Columns.Count].Value = sourceRange.Value;
                }


                // Save and close the first workbook

                // Save and close the workbook
                string fname = Path.GetFileNameWithoutExtension(firstFilePath);
                string extension = Path.GetExtension(firstFilePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }
                string csvFilePath = outputFolder + @"\" + fname + extension;
                firstWorkbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
            finally
            {
                methodCount++;
                // Close the workbooks and release resources
                firstWorkbook?.Close(false);
                secondWorkbook?.Close(false);
                excelApp.Quit();

                // Release COM objects
                if (firstWorkbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(firstWorkbook);
                if (secondWorkbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(secondWorkbook);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }



        static void ReplaceCharacterWithEmptyInColumn(string filePath)
        {
            Application excelApp = new Application();
            Workbook workbook = null;
            Worksheet worksheet = null;
            excelApp.DisplayAlerts = false;

            string[] arr = args.Split('#');
            string headerText = arr[0];
            string targetString = arr[1];

            try
            {
                // Open the workbook
                workbook = excelApp.Workbooks.Open(filePath);
                worksheet = workbook.Sheets[1];

                // Find the column with the specified header text
                Range headerRow = worksheet.Rows[1];
                int columnIndex = -1;
                foreach (Range cell in headerRow.Cells)
                {
                    if (cell.Value2 != null && cell.Value2.ToString() == headerText)
                    {
                        columnIndex = cell.Column;
                        break;
                    }
                }

                if (columnIndex == -1)
                {
                    Console.WriteLine($"Header '{headerText}' not found.");
                    return;
                }

                // Get the last used row in the worksheet
                int lastRow = worksheet.UsedRange.Rows.Count;

                // Loop through each cell in the specified column up to the last used row
                for (int rowIndex = 2; rowIndex <= lastRow; rowIndex++)
                {
                    Range cell = worksheet.Cells[rowIndex, columnIndex];
                    if (cell.Value2 != null)
                    {
                        string cellValue = cell.Value2.ToString();
                        string newValue = cellValue.Replace(targetString, string.Empty);
                        cell.Value2 = newValue;
                    }
                }

                // Save and close the workbook
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }
                string csvFilePath = outputFolder + @"\" + fname + extension;


                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
            }
            finally
            {
                methodCount++;
                // Clean up
                workbook?.Close();
                excelApp.Quit();
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

            }
        }

        static void RunExe(string filePath)
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string[] arr = args.Split('#');
            string exePartialFilePath = arr[0];
            string exePath = currentDirectory + exePartialFilePath;


            try
            {
                // Create a new process start info
                ProcessStartInfo processInfo = new ProcessStartInfo()
                {
                    FileName = exePath,
                    // Arguments = arguments, // Pass arguments to the exe if needed
                    UseShellExecute = false, // Run the process in the current shell
                    CreateNoWindow = true,   // Hide the window of the process
                    RedirectStandardOutput = true, // Capture standard output
                    RedirectStandardError = true   // Capture standard error
                };

                // Start the process
                using (Process process = Process.Start(processInfo))
                {
                    // Capture and display output (optional)
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    // Wait for the process to exit
                    process.WaitForExit();

                    // Optionally print the output and errors
                    Console.WriteLine("Output: " + output);
                    if (!string.IsNullOrEmpty(error))
                    {
                        Console.WriteLine("Error: " + error);
                    }

                    Console.WriteLine($"Process exited with code {process.ExitCode}");
                }
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("An error occurred while running the executable: " + ex.Message);
            }
        }

        static void RunBatchFile(string filePath)
        {
            try
            {
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string[] arr = args.Split('#');
                string partialScriptPath = arr[0];
                string scriptFileName = arr[1];
                string fileName = arr[2];
                string scriptFolderPath = currentDirectory + partialScriptPath + "\\";
                string sourceFolderPath = inputFolder;
                string destinationFolderPath = outputFolder;





                string scriptFilePath = Path.Combine(scriptFolderPath, scriptFileName);
                string scriptrunpath = Path.Combine(inputFolder, scriptFileName);

                string sourceFilePath = Path.Combine(sourceFolderPath, fileName);

                string destinationFilePath = Path.Combine(destinationFolderPath, fileName);

                MoveScriptFile(scriptFilePath, sourceFolderPath, scriptFileName);
                ExecuteBatch(scriptrunpath);
                // RunScript(scriptrunpath);

                MoveFileToNewFolder(sourceFilePath, destinationFilePath);
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
        }


        public static void CopyColumnValuesBasedOnCriteria(string sourceFilePath)
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string[] arr = args.Split('#');
            string destinationFilePath = "";



            string partialFileName = arr[1];
            int criteriaColumnIndex = int.Parse(arr[2]);
            string criteriaText = arr[3];
            int headerRowIndex = int.Parse(arr[4]);
            string valueColumnHeaderText = arr[5];


            string partialFilePath = currentDirectory;
            int destinationFirstColumnIndex = int.Parse(arr[6]);
            string destinationFirstSearchText = arr[7];
            int destinationSymbolColumnIndex = int.Parse(arr[8]);
            string destinationSymbolText = arr[9];
            int destinationPasteColumnIndex = int.Parse(arr[10]);
            string criteriaText2 = arr[11];
            if (!arr[0].Equals("NA"))
            {
                partialFilePath = currentDirectory + arr[0];
            }

            string[] matchingFiles = FindFilesWithPartialName(partialFilePath, partialFileName);
            destinationFilePath = matchingFiles[0];
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook sourceWorkbook = null;
            Excel.Workbook destinationWorkbook = null;
            excelApp.DisplayAlerts = false;

            try
            {
                excelApp.Visible = false;

                // Open the source workbook
                sourceWorkbook = excelApp.Workbooks.Open(sourceFilePath);
                Excel.Worksheet sourceWorksheet = (Excel.Worksheet)sourceWorkbook.Sheets[1];

                // Open the destination workbook
                destinationWorkbook = excelApp.Workbooks.Open(destinationFilePath);
                Excel.Worksheet destinationWorksheet = (Excel.Worksheet)destinationWorkbook.Sheets[1];

                // Find the value column index by header text
                Excel.Range headerRow = sourceWorksheet.Rows[headerRowIndex];
                int valueColumnIndex = -1;
                for (int col = 1; col <= headerRow.Columns.Count; col++)
                {
                    Excel.Range headerCell = (Excel.Range)headerRow.Cells[1, col];
                    if (headerCell.Text.ToString() == valueColumnHeaderText)
                    {
                        valueColumnIndex = col;
                        break;
                    }
                }

                if (valueColumnIndex == -1)
                {
                    throw new Exception("Value column with the specified header text not found.");
                }

                // Find the last row with data in the source sheet
                Excel.Range sourceRange = sourceWorksheet.UsedRange;
                int lastRow = sourceRange.Rows.Count;

                // Loop through each row in the source sheet
                for (int i = headerRowIndex + 1; i <= lastRow; i++)
                {
                    // Get the value from the criteria column
                    Excel.Range criteriaCell = (Excel.Range)sourceWorksheet.Cells[i, criteriaColumnIndex];
                    string criteriaCellValue = criteriaCell.Text.ToString();

                    // Check if the criteria column value matches the criteria text
                    if (criteriaCellValue == criteriaText)
                    {
                        // Get the value from the value column
                        Excel.Range valueCell = (Excel.Range)sourceWorksheet.Cells[i, valueColumnIndex];
                        string valueCellValue = valueCell.Text.ToString();
                        string cellValue2 = GetCellValueBasedOnCondition(sourceFilePath, criteriaColumnIndex, criteriaText2, valueColumnIndex);
                        if (cellValue2 != null && valueCellValue != null)
                        {
                            double temp = double.Parse(valueCellValue) + double.Parse(cellValue2);
                            valueCellValue = temp.ToString();
                        }

                        // Find the destination row based on two criteria
                        int destinationRowIndex = FindRowBasedOnTwoCriteria(destinationFilePath, destinationFirstColumnIndex, destinationFirstSearchText, destinationSymbolColumnIndex, destinationSymbolText);
                        if (destinationRowIndex != -1)
                        {
                            // Paste the value in the corresponding row in the destinationPasteColumnIndex
                            Excel.Range destinationPasteCell = (Excel.Range)destinationWorksheet.Cells[destinationRowIndex, destinationPasteColumnIndex];
                            destinationPasteCell.Value2 = valueCellValue;
                        }
                    }
                }

                // Save and close the destination workbook
                string fname = Path.GetFileNameWithoutExtension(destinationFilePath);
                string extension = Path.GetExtension(destinationFilePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }
                string csvFilePath = outputFolder + @"\" + fname + extension;
                destinationWorkbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("An error occurred: " + ex.Message);
                CreateLogs("An error occurred: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                // Close the workbooks
                if (sourceWorkbook != null)
                {
                    sourceWorkbook.Close(false);
                    Marshal.ReleaseComObject(sourceWorkbook);
                }
                if (destinationWorkbook != null)
                {
                    destinationWorkbook.Close(true);
                    Marshal.ReleaseComObject(destinationWorkbook);
                }

                // Quit the Excel application
                excelApp.Quit();
                Marshal.ReleaseComObject(excelApp);
            }
        }

        public static void SubtractAndPasteValues(string filePath)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = null;
            excelApp.DisplayAlerts = false;
            string[] arr = args.Split('#');
            int destinationFirstColumnIndex = int.Parse(arr[0]);
            string destinationFirstSearchText = arr[1];
            int destinationSymbolColumnIndex = int.Parse(arr[2]);
            string destinationSymbolText = arr[3];
            int firstColumnIndex = int.Parse(arr[4]);
            int secondColumnIndex = int.Parse(arr[5]);
            int resultColumnIndex = int.Parse(arr[6]);
            int rowIndex = FindRowBasedOnTwoCriteria(filePath, destinationFirstColumnIndex, destinationFirstSearchText, destinationSymbolColumnIndex, destinationSymbolText);
            try
            {
                excelApp.Visible = false;

                // Open the workbook
                workbook = excelApp.Workbooks.Open(filePath);
                Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1];

                // Get the values from the specified columns
                Excel.Range firstCell = (Excel.Range)worksheet.Cells[rowIndex, firstColumnIndex];
                Excel.Range secondCell = (Excel.Range)worksheet.Cells[rowIndex, secondColumnIndex];

                // Convert the cell values to double
                double firstValue = Convert.ToDouble(firstCell.Value2);
                double secondValue = Convert.ToDouble(secondCell.Value2);

                // Calculate the result
                double resultValue = firstValue - secondValue;

                // Paste the result into the specified result column
                Excel.Range resultCell = (Excel.Range)worksheet.Cells[rowIndex, resultColumnIndex];
                resultCell.Value2 = resultValue;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("An error occurred: " + ex.Message);
                CreateLogs("An error occurred: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                // Save and close the workbook
                if (workbook != null)
                {
                    string fname = Path.GetFileNameWithoutExtension(filePath);
                    string extension = Path.GetExtension(filePath);
                    if (newfilename != "")
                    {
                        fname = newfilename;
                    }
                    string csvFilePath = outputFolder + @"\" + fname + extension;
                    workbook.SaveAs(csvFilePath);
                    workbook.Close(false);
                    if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                }

                // Quit the Excel application
                excelApp.Quit();

                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        public static string GetCellValueBasedOnCondition(string filePath, int searchColumnIndex, string searchText, int returnColumnIndex)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = null;
            string returnValue = null;
            excelApp.DisplayAlerts = false;

            try
            {
                excelApp.Visible = false;

                // Open the workbook
                workbook = excelApp.Workbooks.Open(filePath);
                Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1];

                // Find the last row with data in the sheet
                Excel.Range usedRange = worksheet.UsedRange;
                int lastRow = usedRange.Rows.Count;

                // Loop through each row in the sheet
                for (int i = 1; i <= lastRow; i++)
                {
                    // Get the value from the search column
                    Excel.Range searchCell = (Excel.Range)worksheet.Cells[i, searchColumnIndex];
                    string searchCellValue = searchCell.Text.ToString();

                    // Check if the search column value matches the search text
                    if (searchCellValue == searchText)
                    {
                        // Get the value from the return column
                        Excel.Range returnCell = (Excel.Range)worksheet.Cells[i, returnColumnIndex];
                        returnValue = returnCell.Text.ToString();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("An error occurred: " + ex.Message);
                CreateLogs("An error occurred: " + ex.Message, logFilePath);
            }
            finally
            {
                // Close the workbook
                if (workbook != null)
                {
                    workbook.Close(false);
                    Marshal.ReleaseComObject(workbook);
                }

                // Quit the Excel application
                excelApp.Quit();
                Marshal.ReleaseComObject(excelApp);
            }

            return returnValue;
        }

        public static int FindRowBasedOnTwoCriteria(string filePath, int firstColumnIndex, string firstSearchText, int secondColumnIndex, string secondSymbolText)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = null;
            excelApp.DisplayAlerts = false;
            int matchingRowIndex = -1;

            try
            {
                excelApp.Visible = false;

                // Open the workbook
                workbook = excelApp.Workbooks.Open(filePath);
                Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1];

                // Find the last row with data in the sheet
                Excel.Range usedRange = worksheet.UsedRange;
                int lastRow = usedRange.Rows.Count;

                // Loop through each row in the sheet
                for (int i = 1; i <= lastRow; i++)
                {
                    Excel.Range firstCell = (Excel.Range)worksheet.Cells[i, firstColumnIndex];
                    Excel.Range secondCell = (Excel.Range)worksheet.Cells[i, secondColumnIndex];

                    string firstCellValue = firstCell.Text.ToString();
                    string secondCellValue = secondCell.Text.ToString();

                    if (firstCellValue == firstSearchText && secondCellValue == secondSymbolText)
                    {
                        matchingRowIndex = i;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("An error occurred: " + ex.Message);
                CreateLogs("An error occurred: " + ex.Message, logFilePath);
            }
            finally
            {
                // Close the workbook
                if (workbook != null)
                {
                    workbook.Close(false);
                    Marshal.ReleaseComObject(workbook);
                }

                // Quit the Excel application
                excelApp.Quit();
                Marshal.ReleaseComObject(excelApp);
            }

            return matchingRowIndex;
        }


        public static void CopyFirstWorksheetToNewFileWithClearFormat(string filePath)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook sourceWorkbook = null;
            Excel.Workbook destinationWorkbook = null;
            Excel.Worksheet sourceSheet = null;
            Excel.Worksheet destinationSheet = null;
            excelApp.DisplayAlerts = false;
            // Save and close the workbook
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }
            string destinationFilePath = outputFolder + @"\" + fname + extension;

            try
            {
                // Open the source workbook
                sourceWorkbook = excelApp.Workbooks.Open(filePath);

                // Get the first worksheet in the source workbook
                sourceSheet = (Excel.Worksheet)sourceWorkbook.Sheets[1];

                // Find the used range in the first sheet
                Excel.Range usedRange = sourceSheet.UsedRange;

                // Copy the used range
                usedRange.Copy(Type.Missing);

                // Create a new workbook
                destinationWorkbook = excelApp.Workbooks.Add();
                destinationSheet = (Excel.Worksheet)destinationWorkbook.Sheets[1];

                // Paste only the values into the new sheet
                Excel.Range destinationStartCell = destinationSheet.Cells[1, 1];
                destinationStartCell.PasteSpecial(Excel.XlPasteType.xlPasteValues, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, Type.Missing, Type.Missing);

                // Clear the formatting of the pasted range
                Excel.Range pastedRange = destinationSheet.UsedRange;
                pastedRange.ClearFormats();

                // Optionally, you can adjust the column widths to match the original
                destinationSheet.Columns.AutoFit();

                // Save the new workbook
                destinationWorkbook.SaveAs(destinationFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
            finally
            {
                methodCount++;
                // Cleanup
                if (sourceWorkbook != null) sourceWorkbook.Close(false);
                if (destinationWorkbook != null) destinationWorkbook.Close(false);
                excelApp.Quit();

                // Release COM objects
                if (sourceSheet != null) Marshal.ReleaseComObject(sourceSheet);
                if (destinationSheet != null) Marshal.ReleaseComObject(destinationSheet);
                if (sourceWorkbook != null) Marshal.ReleaseComObject(sourceWorkbook);
                if (destinationWorkbook != null) Marshal.ReleaseComObject(destinationWorkbook);
                Marshal.ReleaseComObject(excelApp);
            }
        }





        static void ClearFormatfast(string filePath)
        {
            string logmessage = "";
            Excel.Workbook workbook = null;
            Excel.Application excelApp = null;
            Excel.Worksheet worksheet = null;
            string fname = Path.GetFileName(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname;
            try
            {

                excelApp = new Excel.Application();

                excelApp.DisplayAlerts = false; // Disable alerts to avoid any pop-ups during processing

                workbook = excelApp.Workbooks.Open(filePath);

                // Iterate through all sheets
                foreach (Excel.Worksheet sheet in workbook.Sheets)
                {
                    // Clear formats for the entire used range
                    sheet.UsedRange.ClearFormats();
                }

                // Save the changes back to the file


                // workbook.Save();
                workbook.SaveAs(csvFilePath);
                workbook.Close(true);
                excelApp.Quit();

                logmessage += Environment.NewLine + $"Cell styles cleared successfully in file: {filePath}";
                CreateLogs($"Cell styles cleared successfully in file: {filePath}", logFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                logmessage += Environment.NewLine + $"Error clearing format of sheet in file '{filePath}': {ex.Message}";
                LogWithRed(logmessage);
                CreateLogs(logmessage, logFilePath);

            }
            finally
            {
                methodCount++;
                // Close and release resources
                //workbook.Close(false);
                //excelApp.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }

            Console.WriteLine(logmessage);

            CreateLogs(logmessage, logFilePath);
        }

        public static void DeleteColumnsWithHeaderText(Excel.Worksheet worksheet, string headerText, string headerRowIndex)
        {
            try
            {
                // Get the used range of the worksheet
                Excel.Range usedRange = worksheet.UsedRange;

                // Find the header row (assuming it's the first row)
                Excel.Range headerRow = usedRange.Rows[headerRowIndex];

                // Loop through the columns in the header row
                for (int col = headerRow.Columns.Count; col >= 1; col--)
                {
                    Excel.Range cell = headerRow.Cells[1, col] as Excel.Range;

                    if (cell.Value2 != null && cell.Value2.ToString().Trim() == headerText)
                    {
                        // Delete the entire column
                        Excel.Range column = cell.EntireColumn;
                        column.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);
                    }
                }
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("An error occurred: " + ex.Message);
                CreateLogs("An error occurred: " + ex.Message, logFilePath);
            }
        }



        public static void DeleteColumnsWithHeaderTextFromExcel(string filePath)
        {
            string[] arr = args.Split('#');


            string headerText = arr[0];
            string headerRowIndex = arr[1];
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;
            excelApp.DisplayAlerts = false;

            try
            {
                // Open the workbook
                workbook = excelApp.Workbooks.Open(filePath);

                // Get the first worksheet (or specify the sheet you want to work with)
                worksheet = (Excel.Worksheet)workbook.Sheets[1];

                // Call the method to delete columns with the specified header text
                DeleteColumnsWithHeaderText(worksheet, headerText, headerRowIndex);

                // Save and close the workbook
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }
                string csvFilePath = outputFolder + @"\" + fname + extension;
                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
            finally
            {
                methodCount++;
                // Cleanup
                workbook?.Close(false);
                excelApp.Quit();

                // Release COM objects
                if (worksheet != null) Marshal.ReleaseComObject(worksheet);
                if (workbook != null) Marshal.ReleaseComObject(workbook);
                Marshal.ReleaseComObject(excelApp);
            }
        }


        public static int GetRowIndexOfFirstOccurrence(string filePath, int columnNumber, string cellValue)
        {
            Excel.Application excelApp = null;
            Excel.Workbooks workbooks = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;


            try
            {
                // Initialize Excel application
                excelApp = new Excel.Application();
                workbooks = excelApp.Workbooks;
                workbook = workbooks.Open(filePath);
                worksheet = workbook.Sheets[1] as Excel.Worksheet;

                // Get the range of the specified column
                Excel.Range columnRange = worksheet.Columns[columnNumber];

                // Find the first occurrence of the cell value
                Excel.Range foundCell = columnRange.Find(
                    What: cellValue,
                    LookIn: Excel.XlFindLookIn.xlValues,
                    LookAt: Excel.XlLookAt.xlWhole,
                    SearchOrder: Excel.XlSearchOrder.xlByRows,
                    SearchDirection: Excel.XlSearchDirection.xlNext,
                    MatchCase: false);

                if (foundCell != null)
                {
                    return foundCell.Row;
                }
                else
                {
                    // Return -1 if the value is not found
                    return -1;
                }
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
                return -1;
            }
            finally
            {
                // Clean up
                if (workbook != null) workbook.Close(false);
                if (workbooks != null) workbooks.Close();
                if (excelApp != null) excelApp.Quit();

                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (workbooks != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbooks);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

            }
        }


        public static void DeletePdfFiles(string filepath)
        {
            Console.WriteLine("Method count before Deleted PDF is :" + methodCount);
            string directoryPath = inputFolder;
            try
            {
                // Check if directory exists
                if (!Directory.Exists(directoryPath))
                {
                    Console.WriteLine("Directory does not exist.");
                    CreateLogs("Directory does not exist.", logFilePath);
                    return;
                }

                // Get all PDF files in the directory
                string[] pdfFiles = Directory.GetFiles(directoryPath, "*.pdf");

                // Delete each PDF file
                foreach (string pdfFile in pdfFiles)
                {
                    File.Delete(pdfFile);
                }
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode++;
                LogWithRed($"An error occurred: {ex.Message}");
                CreateLogs($"An error occurred: {ex.Message}", logFilePath);
            }
        }
        public static void CutPdfFiles(string filepath)
        {
            string sourceDirectory = inputFolder;
            string destinationDirectory = outputFolder;
            try
            {
                // Check if source directory exists
                if (!Directory.Exists(sourceDirectory))
                {
                    Console.WriteLine("Source directory does not exist.");
                    CreateLogs("Source directory does not exist.", logFilePath);
                    return;
                }

                // Check if destination directory exists, if not, create it
                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }

                // Get all PDF files in the source directory
                string[] pdfFiles = Directory.GetFiles(sourceDirectory, "*.pdf");

                // Move each PDF file to the destination directory
                foreach (string pdfFile in pdfFiles)
                {
                    string fileName = Path.GetFileName(pdfFile);
                    string destinationFile = Path.Combine(destinationDirectory, fileName);

                    File.Move(pdfFile, destinationFile);
                }
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed($"An error occurred: {ex.Message}");
                CreateLogs($"An error occurred: {ex.Message}", logFilePath);
            }
        }


        public static void ClearLeadingTrailingEmptyStringsOfParticularColumn(string filePath)
        {
            Excel.Application excelApp = new Excel.Application();
            excelApp.DisplayAlerts = false;
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet worksheet = workbook.ActiveSheet;
            excelApp.DisplayAlerts = false;

            string[] arr = args.Split('#');
            string headerText = arr[0];
            string colNo = "";
            if (arr.Length > 1)
                colNo = arr[1];

            try
            {
                // Find the column with the specified header text
                Excel.Range headerRow = worksheet.Rows[1];
                int columnIndex = -1;
                foreach (Excel.Range cell in headerRow.Cells)
                {
                    if (cell.Value2 != null && cell.Value2.ToString() == headerText)
                    {
                        columnIndex = cell.Column;
                        break;
                    }
                }

                if (columnIndex == -1)
                {
                    Console.WriteLine($"Header '{headerText}' not found.");
                    CreateLogs($"Header '{headerText}' not found.", logFilePath);
                    columnIndex = int.Parse(colNo);
                    // return;
                }

                // Get the last used row in the worksheet
                int lastRow = worksheet.UsedRange.Rows.Count;

                // Loop through each cell in the specified column up to the last used row
                for (int rowIndex = 1; rowIndex <= lastRow; rowIndex++)
                {
                    Excel.Range cell = worksheet.Cells[rowIndex, columnIndex];
                    if (cell.Value2 != null && cell.Value2 is string)
                    {
                        string trimmedValue = ((string)cell.Value2).Trim();

                        if (trimmedValue != (string)cell.Value2)
                        {
                            cell.Value2 = trimmedValue;
                        }
                    }
                }

                // Save and close the workbook
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (!string.IsNullOrEmpty(newfilename))
                {
                    fname = newfilename;
                }
                string csvFilePath = Path.Combine(outputFolder, fname + extension);
                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();

                // Release COM objects to avoid memory leaks
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

                // Garbage collection
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        public static void ClearLeadingTrailingEmptyStrings(string filePath)
        {
            try
            {
                Excel.Application excelApp = new Excel.Application();
                excelApp.DisplayAlerts = false;
                Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
                Excel.Worksheet worksheet = workbook.ActiveSheet;

                // Get the range of used cells in the worksheet
                Excel.Range usedRange = worksheet.UsedRange;

                // Loop through each cell in the used range
                foreach (Excel.Range cell in usedRange)
                {
                    // Check if the cell contains a string value
                    if (cell.Value != null && cell.Value is string)
                    {
                        // Trim leading and trailing spaces from the cell value
                        string trimmedValue = ((string)cell.Value).Trim();

                        // Update the cell value if it has leading or trailing spaces
                        if (trimmedValue != (string)cell.Value)
                        {
                            cell.Value = trimmedValue;
                        }
                    }
                }

                // Save and close the workbook
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }
                string csvFilePath = outputFolder + @"\" + fname + extension;
                workbook.SaveAs(csvFilePath);
                workbook.Close();

                // Quit Excel application
                excelApp.Quit();

                // Release COM objects to avoid memory leaks
                System.Runtime.InteropServices.Marshal.ReleaseComObject(usedRange);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

                // Garbage collection
                GC.Collect();
                GC.WaitForPendingFinalizers();
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
        }
        public static void CopyRowFromOneFileToOther(string destinationFilePath)
        {
            try
            {
                string[] arr = args.Split('#');
                string sourceFilePath = Directory.GetCurrentDirectory() + "\\" + arr[0];
                int sourceRowIndex = int.Parse(arr[1]);
                int destinationRowIndex = int.Parse(arr[2]);
                Excel.Application excelApp = new Excel.Application();
                excelApp.DisplayAlerts = false;
                Excel.Workbook sourceWorkbook = excelApp.Workbooks.Open(sourceFilePath);
                Excel.Workbook destinationWorkbook = excelApp.Workbooks.Open(destinationFilePath);

                // Get source and destination worksheets
                Excel.Worksheet sourceWorksheet = (Excel.Worksheet)sourceWorkbook.ActiveSheet;
                Excel.Worksheet destinationWorksheet = (Excel.Worksheet)destinationWorkbook.ActiveSheet;

                // Copy the specific row from the source worksheet
                Excel.Range sourceRange = (Excel.Range)sourceWorksheet.Rows[sourceRowIndex];
                sourceRange.Copy();

                // Paste it into the destination worksheet at the specified row index
                Excel.Range destinationRange = (Excel.Range)destinationWorksheet.Rows[destinationRowIndex];
                destinationRange.Insert(Excel.XlInsertShiftDirection.xlShiftDown);

                // Save the destination workbook
                // Save the workbook
                string fname = Path.GetFileNameWithoutExtension(destinationFilePath);
                string extension = Path.GetExtension(destinationFilePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }
                string csvFilePath = outputFolder + @"\" + fname + extension;

                destinationWorkbook.SaveAs(csvFilePath);
                // Close the workbooks without saving changes to the source workbook
                sourceWorkbook.Close();
                destinationWorkbook.Close();
                excelApp.Quit();
                // Release COM objects
                System.Runtime.InteropServices.Marshal.ReleaseComObject(destinationRange);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(sourceRange);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(destinationWorksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(sourceWorksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(destinationWorkbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(sourceWorkbook);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
        }


        static void SetCellValue(string filePath)
        {
            try
            {

                string[] arr = args.Split('#');
                int row = int.Parse(arr[0]);
                int col = int.Parse(arr[1]);
                object value = arr[2];
                // Create a new Excel application
                Excel.Application excelApp = new Excel.Application();

                excelApp.DisplayAlerts = false;

                // Open the workbook
                Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);

                // Get the first worksheet
                Excel.Worksheet worksheet = workbook.Sheets[1]; // Index is 1-based

                // Set the value in the specified cell
                worksheet.Cells[row, col] = value;

                // Save the workbook
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }
                string csvFilePath = outputFolder + @"\" + fname + extension;

                workbook.SaveAs(csvFilePath);

                // Close the workbook and release resources
                workbook.Close();
                excelApp.Quit();

                // Release COM objects to avoid memory leaks
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

                // Optional: Force garbage collection to free up resources immediately
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
        }

        public static void DeleteAllFilesInFolder(string folderPath)
        {
            folderPath = inputFolder;
            try
            {
                // Check if the directory exists
                if (Directory.Exists(folderPath))
                {
                    // Get all files in the directory
                    string[] files = Directory.GetFiles(folderPath);

                    // Delete each file
                    foreach (string file in files)
                    {
                        File.Delete(file);
                    }

                    Console.WriteLine("All files deleted successfully.");
                    CreateLogs("All files deleted successfully.", logFilePath);
                }
                else
                {
                    Console.WriteLine("Directory does not exist.");
                    CreateLogs("Directory does not exist.", logFilePath);
                }
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("An error occurred: " + ex.Message);
                CreateLogs("An error occurred: " + ex.Message, logFilePath);
            }
        }

        public static void PerformDivisionBetweenTwoColumn(string filePath)
        {
            try
            {
                Excel.Application excelApp = new Excel.Application();
                excelApp.DisplayAlerts = false;
                Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
                Excel.Worksheet worksheet = workbook.Sheets[1]; // Assuming the data is on the first sheet

                Excel.Range usedRange = worksheet.UsedRange;
                Excel.Range headerRange = usedRange.Rows[1];
                string[] arr = args.Split('#');
                string column1HeaderText = arr[0];
                string column2HeaderText = arr[1];
                int resultColumnIndex = int.Parse(arr[2]);

                int column1Index = 0;
                int column2Index = 0;

                // Find column indices based on header text
                foreach (Excel.Range cell in headerRange.Cells)
                {
                    if (cell.Value2 != null)
                    {
                        if (cell.Value2.ToString() == column1HeaderText)
                            column1Index = cell.Column;
                        else if (cell.Value2.ToString() == column2HeaderText)
                            column2Index = cell.Column;
                    }
                }

                // Perform division and put result in specified column
                for (int i = 2; i <= usedRange.Rows.Count; i++) // Assuming data starts from row 2
                {
                    Excel.Range cell1 = (Excel.Range)worksheet.Cells[i, column1Index];
                    Excel.Range cell2 = (Excel.Range)worksheet.Cells[i, column2Index];
                    Excel.Range resultCell = (Excel.Range)worksheet.Cells[i, resultColumnIndex];

                    double value1 = Convert.ToDouble(cell1.Value2);
                    double value2 = Convert.ToDouble(cell2.Value2);

                    if (value2 != 0)
                    {
                        double result = value1 / value2;
                        resultCell.Value2 = result;
                    }
                    else
                    {
                        resultCell.Value2 = "";
                    }
                }


                // Save the workbook
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }
                string csvFilePath = outputFolder + @"\" + fname + extension;

                workbook.SaveAs(csvFilePath);
                workbook.Close();
                excelApp.Quit();

                ReleaseObject(worksheet);
                ReleaseObject(workbook);
                ReleaseObject(excelApp);
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
        }



        public static void InsertBlankColumn(string filePath)
        {
            try
            {
                string[] arr = args.Split('#');
                int columnIndex = int.Parse(arr[0]);
                Excel.Application excelApp = new Excel.Application();
                excelApp.DisplayAlerts = false;
                Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
                Excel.Worksheet worksheet = (Excel.Worksheet)workbook.ActiveSheet;

                // Shift cells to the right to make room for the new column
                Excel.Range range = worksheet.Columns[columnIndex];
                range.Insert(Excel.XlInsertShiftDirection.xlShiftToRight, Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);

                // Save the workbook
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }
                string csvFilePath = outputFolder + @"\" + fname + extension;

                workbook.SaveAs(csvFilePath);

                // Release COM objects
                System.Runtime.InteropServices.Marshal.ReleaseComObject(range);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
        }

        public static void InsertBlankRowAtTop(string filePath)
        {
            try
            {
                Excel.Application excelApp = new Excel.Application();
                excelApp.DisplayAlerts = false;
                excelApp.Visible = false; // For debugging

                Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
                Excel.Worksheet worksheet = (Excel.Worksheet)workbook.ActiveSheet;

                // Unprotect the sheet if necessary
                if (worksheet.ProtectContents)
                {
                    worksheet.Unprotect();
                }

                // Unmerge cells and ensure row 1 is visible
                Excel.Range firstRowRange = worksheet.Rows[1];
                if (firstRowRange.MergeCells)
                {
                    firstRowRange.UnMerge();
                }
                firstRowRange.EntireRow.Hidden = false;
                worksheet.Application.ActiveWindow.FreezePanes = false;

                // Insert a blank row at row 2, then shift everything down
                Excel.Range row2Range = worksheet.Rows[2];
                row2Range.Insert(Excel.XlInsertShiftDirection.xlShiftDown, Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);

                // Now copy the contents of row 1 into row 2
                worksheet.Rows[1].Copy(worksheet.Rows[2]);

                // Clear the original row 1 (now blank)
                worksheet.Rows[1].Clear();

                // Save the workbook
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (!string.IsNullOrEmpty(newfilename))
                {
                    fname = newfilename;
                }
                string csvFilePath = string.IsNullOrEmpty(outputFolder)
                    ? Path.Combine(Path.GetDirectoryName(filePath), fname + extension)
                    : Path.Combine(outputFolder, fname + extension);

                workbook.SaveAs(csvFilePath);

                // Clean up and release COM objects
                workbook.Close(false);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
        }


        public static void InsertBlankRow(string filePath)
        {

            string[] arr = args.Split('#');
            int rowIndex = int.Parse(arr[0]);
            Excel.Application excelApp = new Excel.Application();
            excelApp.DisplayAlerts = false;
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.ActiveSheet;

            // Shift cells down to make room for the new row at the specified index
            Excel.Range range = worksheet.Rows[rowIndex];

            // Ensure the range is valid and then insert the row
            try
            {
                range.Insert(Excel.XlInsertShiftDirection.xlShiftDown, Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
            }
            catch (Exception ex)
            {
                LogWithRed("Error inserting row: " + ex.Message);
            }
            try
            {
                // Save the workbook
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }
                string csvFilePath = outputFolder + @"\" + fname + extension;

                workbook.SaveAs(csvFilePath);

                // Release COM objects
                System.Runtime.InteropServices.Marshal.ReleaseComObject(range);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
        }

        public static void MoveColumnToFirstPosition(string excelFilePath)
        {
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;
            string[] arr = args.Split('#');
            string headerText = arr[0];
            try
            {
                excelApp = new Excel.Application();
                excelApp.DisplayAlerts = false;
                workbook = excelApp.Workbooks.Open(excelFilePath);
                worksheet = workbook.Sheets[1];

                Excel.Range usedRange = worksheet.UsedRange;
                Excel.Range headerRow = usedRange.Rows[1];
                Excel.Range foundHeaderCell = headerRow.Find(headerText, Type.Missing,
    Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlWhole, Excel.XlSearchOrder.xlByColumns,
    Excel.XlSearchDirection.xlNext, false, false, Type.Missing);


                if (foundHeaderCell != null)
                {

                    // Calculate the absolute column index in the worksheet
                    int currentColumn = foundHeaderCell.Column;

                    // Move the column to the first position
                    worksheet.Columns[currentColumn].Cut(worksheet.Columns[1]);

                    // Delete the initial column
                    Excel.Range initialColumn = (Excel.Range)worksheet.Columns[currentColumn];
                    initialColumn.Delete();

                    // Save the workbook
                    string fname = Path.GetFileNameWithoutExtension(excelFilePath);
                    string extension = Path.GetExtension(excelFilePath);
                    if (newfilename != "")
                    {
                        fname = newfilename;
                    }
                    string csvFilePath = outputFolder + @"\" + fname + extension;

                    workbook.SaveAs(csvFilePath);


                    Console.WriteLine("Column moved successfully.");
                }
                else
                {
                    Console.WriteLine("Header not found.");
                }

            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();
                ReleaseObject(worksheet);
                ReleaseObject(workbook);
                ReleaseObject(excelApp);
            }
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
                        LogWithRed("Failed to extract FilterValue.");
                        CreateLogs("Failed to extract FilterValue.", logFilePath);
                    }

                }
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
                CreateLogs(ex.Message.ToString(), logFilePath);
            }
            return DateTime.MinValue;
        }

        //public static string ReplaceDateTimePlaceholders(string input)
        //{
        //    string pattern = @"\(([^)]+)\)";
        //    Regex regex = new Regex(pattern);

        //    MatchCollection matches = regex.Matches(input);

        //    foreach (Match match in matches)
        //    {
        //        string placeholder = match.Groups[1].Value;
        //        string replacement = GetFormattedDateTime(placeholder);
        //        input = input.Replace(match.Value, replacement);
        //    }

        //    return input;
        //}

        public static string GetFormattedDateTime(string format, int dateFactor)
        {
            DateTime modifiedDate = processDate.AddDays(dateFactor);

            // Convert the modified date to string with the required format
            string formattedDate = modifiedDate.ToString(format);

            return formattedDate;
        }


        public static string GetFormattedDateTimeCurrentDate(string format, int dateFactor)
        {
            DateTime currentDate = DateTime.Now;
            // Add the dateFactor to the currentDate object
            DateTime modifiedDate = currentDate.AddDays(dateFactor);

            // Convert the modified date to string with the required format
            string formattedDate = modifiedDate.ToString(format);

            return formattedDate;
        }
        public static string ReplaceDateTimePlaceholders(string input)
        {
            string dateFactors = "";
            int i = 0;
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
                LogWithRed(ex.Message.ToString());
                CreateLogs(ex.Message.ToString(), logFilePath);
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
                exitcode = 5;
                LogWithRed($"Error reading XML path from config: {ex.Message}");
                CreateLogs($"Error reading XML path from config: {ex.Message}", logFilePath);
                return null;
            }
        }


        public static void DeleteRowsInRange(string filePath)
        {

            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(filePath);

            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet worksheet = workbook.ActiveSheet;
            excelApp.DisplayAlerts = false;
            string[] arr = args.Split('#');

            int startIndex = int.Parse(arr[0]);
            int endIndex;

            // Get the used range in the worksheet
            Excel.Range usedRange = worksheet.UsedRange;

            // If endIndex is not provided or is empty, set it to the last used row
            if (string.IsNullOrEmpty(arr[1]))
            {
                endIndex = usedRange.Rows.Count;
            }
            else
            {
                endIndex = int.Parse(arr[1]);
            }
            try
            {
                // Ensure start index and end index are within the range of rows in the worksheet
                if (startIndex >= 1 && startIndex <= endIndex && endIndex <= usedRange.Rows.Count)
                {
                    // Delete rows starting from endIndex and work backwards to startIndex
                    for (int rowIndex = endIndex; rowIndex >= startIndex; rowIndex--)
                    {
                        Excel.Range row = (Excel.Range)worksheet.Rows[rowIndex];
                        row.Delete();
                    }

                    // Save the workbook
                    workbook.SaveAs(csvFilePath);
                }
                else
                {
                    Console.WriteLine("Invalid start index or end index.");
                    CreateLogs("Invalid start index or end index.", logFilePath);
                }

            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }
        public static void PNC(string filePath)
        {
            string modifiedFileFolder = "";
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = null;
            excelApp.DisplayAlerts = false;
            try
            {

                workbook = excelApp.Workbooks.Open(filePath);
            }
            catch (Exception ex)
            {
                LogWithRed(ex.Message.ToString());
                return;
            }

            try
            {
                int startRow = 8;//First Equity is written to be copied...
                int columnIndex = 11;
                //for (int sheetIndex = 0; sheetIndex < workbook.Sheets.Count; sheetIndex++)
                //{
                Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1];
                int rowCount = worksheet.UsedRange.Rows.Count;
                int blankCellCount = 0;
                for (int row = startRow; row <= rowCount; row++)
                {
                    Excel.Range cell = (Excel.Range)worksheet.Cells[row, columnIndex];
                    if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                    {
                        blankCellCount++;
                    }
                    else
                    {
                        break;
                    }
                }
                CopyAndPaste(worksheet, 2, 2, startRow, columnIndex, startRow + blankCellCount - 1, columnIndex, null);
                string modified_filePath = Path.Combine(modifiedFileFolder, Path.GetFileName(filePath));
                workbook.Save();
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close(false);
                excelApp.Quit();
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        public static void AutoExpandColumn(string filePath)
        {
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            string[] arr = args.Split('#');
            int columnIndex = int.Parse(arr[0]);
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;

            try
            {
                // Initialize Excel application
                excelApp = new Excel.Application();
                excelApp.DisplayAlerts = false;

                // Open the workbook
                workbook = excelApp.Workbooks.Open(filePath);
                worksheet = workbook.Sheets[1] as Excel.Worksheet;

                // Get the last used row in the specified column (corrected Cells property usage)
                int lastUsedRow = worksheet.Cells[worksheet.Rows.Count, columnIndex].End(Excel.XlDirection.xlUp).Row;

                // Get the range from the first row to the last used row in the specified column
                Excel.Range columnRange = worksheet.Range[worksheet.Cells[1, columnIndex], worksheet.Cells[lastUsedRow, columnIndex]];
                // Loop through each cell in the column range to handle the ###### issue
                foreach (Excel.Range cell in columnRange.Cells)
                {
                    if (cell.Value2 != null)
                    {
                        // Check if the cell's value is numeric or a date
                        if (double.TryParse(cell.Value2.ToString(), out double numberValue))
                        {
                            // Convert the cell to a numeric format to avoid it being treated as text
                            cell.NumberFormat = "0.00"; // Or use "General" to keep it flexible
                        }
                        else if (cell.Value2 is DateTime)
                        {
                            // Convert the cell to a date format if it's a date
                            cell.NumberFormat = "m/d/yyyy h:mm"; // Adjust date format as needed
                        }
                    }
                }

                // Apply AutoFit to the specific column
                columnRange.Columns.AutoFit();


                // Save the workbook as a new file
                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
            }
            finally
            {
                methodCount++;
                // Cleanup
                if (workbook != null) workbook.Close();
                if (excelApp != null) excelApp.Quit();

                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
            }
        }

        public static void ConvertColValuetoNumber(string filePath)
        {
            string modifiedFileFolder = "";
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = null;
            excelApp.DisplayAlerts = false;
            try
            {

                workbook = excelApp.Workbooks.Open(filePath);
            }
            catch (Exception ex)
            {
                LogWithRed(ex.Message.ToString());
                return;
            }

            try
            {
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;

                Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1];
                int columnNumber = -1;
                columnNumber = int.Parse(args);
                if (columnNumber > 0)
                {
                    Excel.Range columnRange = worksheet.Columns[columnNumber];

                    columnRange.NumberFormat = "0";
                    columnRange.Value = columnRange.Value2;
                }
                // string modified_filePath = Path.Combine(outputFolder, Path.GetFileName(filePath));
                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close(false);
                excelApp.Quit();
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        public static void AppendZeroToBeginingOfCell(string filePath)
        {
            string modifiedFileFolder = "";
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = null;
            excelApp.DisplayAlerts = false;
            try
            {

                workbook = excelApp.Workbooks.Open(filePath);
            }
            catch (Exception ex)
            {
                LogWithRed(ex.Message.ToString());
                return;


            }

            try
            {
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;

                Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[1];
                int columnIndex = -1;
                string[] arr = args.Split('#');
                columnIndex = int.Parse(arr[0]);
                string searchText = arr[1];
                string[] searchTextArr = searchText.Split(';');
                List<string> searchTextList = searchTextArr.ToList();
                string headerText = arr[2];
                int startRowIndex = int.Parse(arr[3]);
                // Filter and delete rows
                Excel.Range usedRange = sheet.UsedRange;
                int rowCount = usedRange.Rows.Count;
                Excel.Range columnRange = sheet.Columns[columnIndex];
                if (columnIndex > 0)
                {
                    foreach (string val in searchTextList)
                    {
                        Excel.Range foundCells = columnRange.Find(val);
                        if (foundCells != null)
                        {
                            foundCells.Value = "0" + val;
                        }
                    }
                    //Excel.Range valuesRange = columnRange.Cells;
                    //object[,] values = (object[,])valuesRange.Value;

                    //// Iterate through each cell in the column and append "0" at the beginning
                    //for (int i = 1; i <= values.GetLength(0); i++)
                    //{
                    //    string cellValue = values[i, 1]?.ToString(); // Column index is 1-based
                    //    if (!string.IsNullOrEmpty(cellValue) && searchTextList.Contains(cellValue))
                    //    {
                    //        // Append "0" at the beginning of the cell value
                    //        values[i, 1] = "0" + cellValue;
                    //    }
                    //}

                    //// Write the updated values back to the worksheet
                    //columnRange.Value = values;

                }

                // string modified_filePath = Path.Combine(outputFolder, Path.GetFileName(filePath));
                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close(false);
                excelApp.Quit();
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }
        public static void AppendZeroToBeginingOfCell_V2(string filePath)
        {
            string modifiedFileFolder = "";
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = null;
            excelApp.DisplayAlerts = false;
            try
            {

                workbook = excelApp.Workbooks.Open(filePath);
            }
            catch (Exception ex)
            {
                LogWithRed(ex.Message.ToString());
                return;


            }

            try
            {
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;

                Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[1];
                int columnIndex = -1;
                string[] arr = args.Split('#');
                columnIndex = int.Parse(arr[0]);
                string searchText = arr[1];
                string[] searchTextArr = searchText.Split(';');
                List<string> searchTextList = searchTextArr.ToList();
                string headerText = arr[2];
                int startRowIndex = int.Parse(arr[3]);
                // Filter and delete rows
                Excel.Range usedRange = sheet.UsedRange;
                int rowCount = usedRange.Rows.Count;
                Excel.Range columnRange = sheet.Columns[columnIndex];
                object[,] values = columnRange.Value2;
                if (values != null)
                {
                     

                    for (int row = startRowIndex; row <= rowCount; row++)
                    {
                        if (values[row, 1] != null)
                        {
                            string cellValue = values[row, 1].ToString();
                            string modifiedValue = cellValue.StartsWith("0") ? cellValue.TrimStart('0') : cellValue;

                            // Check if modified value matches any item in searchTextList
                            if (searchTextList.Contains(modifiedValue) || searchTextList.Contains(cellValue))
                            {
                                if (cellValue.StartsWith("'0") || cellValue.StartsWith("0"))
                                {
                                    values[row, 1] = "'" + cellValue;
                                }
                                else
                                {
                                    values[row, 1] = "'" + "0" + cellValue; // Apostrophe ensures it's treated as text
                                }
                            }
                        }
                    }

                    // Write updated values back to Excel
                    columnRange.Value2 = values;
                    columnRange.NumberFormat = "@"; // Force column to text format
                }

                // string modified_filePath = Path.Combine(outputFolder, Path.GetFileName(filePath));
                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
               
            }
            finally
            {
                methodCount++;
               
                workbook?.Close(false);
                excelApp.Quit();
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        static void DeleteWorksheet(string excelFilePath)
        {
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            string[] arr = args.Split('#');
            string sheetNameToDelete = arr[0];

            try
            {
                string fname = Path.GetFileNameWithoutExtension(excelFilePath);
                string extension = Path.GetExtension(excelFilePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;
                excelApp = new Excel.Application();
                excelApp.DisplayAlerts = false;

                workbook = excelApp.Workbooks.Open(excelFilePath);

                Excel.Worksheet worksheetToDelete = null;
                foreach (Excel.Worksheet sheet in workbook.Sheets)
                {
                    if (sheet.Name == sheetNameToDelete)
                    {
                        worksheetToDelete = sheet;
                        break;
                    }
                }

                if (worksheetToDelete != null)
                {
                    worksheetToDelete.Delete();
                    Console.WriteLine("Worksheet '{0}' deleted successfully.", sheetNameToDelete);
                    CreateLogs($"Worksheet '{sheetNameToDelete}' deleted successfully.", logFilePath);
                }
                else
                {
                    Console.WriteLine("Worksheet '{0}' not found.", sheetNameToDelete);
                    CreateLogs($"Worksheet '{sheetNameToDelete}' not found.", logFilePath);
                }
                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                // Close and release resources
                workbook.Close(false);
                excelApp.Quit();
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }

        }

        static void DeleteWorksheetExcept(string excelFilePath)
        {
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            string[] arr = args.Split('#');
            string sheetNameToKeep = arr[0];

            try
            {
                string fname = Path.GetFileNameWithoutExtension(excelFilePath);
                string extension = Path.GetExtension(excelFilePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;
                excelApp = new Excel.Application();
                excelApp.DisplayAlerts = false;

                workbook = excelApp.Workbooks.Open(excelFilePath);

                Excel.Worksheet worksheetToDelete = null;
                foreach (Excel.Worksheet sheet in workbook.Sheets)
                {
                    if (!(sheet.Name == sheetNameToKeep))
                    {
                        worksheetToDelete = sheet;
                    }
                }

                if (worksheetToDelete != null)
                {
                    worksheetToDelete.Delete();
                    Console.WriteLine("Worksheet Except '{0}' deleted successfully.", sheetNameToKeep);
                    CreateLogs($"Worksheet Except '{sheetNameToKeep}' deleted successfully.", logFilePath);
                }
                else
                {
                    Console.WriteLine("Worksheet Except '{0}' not found.", sheetNameToKeep);
                    CreateLogs($"Worksheet Except '{sheetNameToKeep}' not found.", logFilePath);
                }
                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                // Close and release resources
                workbook.Close(false);
                excelApp.Quit();
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }

        }



        static void FindCellValueAndReplace(string excelFilePath)
        {
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;
            string[] arr = args.Split('#');
            string targetValue = arr[0];
            string replaceValue = arr[1];

            try
            {
                excelApp = new Excel.Application();
                excelApp.DisplayAlerts = false;

                workbook = excelApp.Workbooks.Open(excelFilePath);
                worksheet = (Excel.Worksheet)workbook.Sheets[1];
                string fname = Path.GetFileNameWithoutExtension(excelFilePath);
                string extension = Path.GetExtension(excelFilePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;

                Excel.Range range = worksheet.UsedRange;
                Excel.Range foundCells = range.Find(targetValue);

                while (foundCells != null)
                {
                    foundCells.Value = replaceValue;
                    foundCells = range.FindNext(foundCells);
                }

                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                // Close and release resources
                workbook.Close(false);
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }

        }

        public static void ReplaceColumnNameInHeader(string filePath)
        {
            Application excelApp = new Application();
            excelApp.DisplayAlerts = false;

            Workbook workbook = excelApp.Workbooks.Open(filePath);
            Worksheet worksheet = (Worksheet)workbook.Sheets[1];

            try
            {
                string[] arr = args.Split('#');
                string targetValue = arr[0]; // Target header name
                string replaceValue = arr[1]; // New header name

                Range usedRange = worksheet.UsedRange;
                Range headerRange = usedRange.Rows[1]; // Get the header row

                // Iterate through header cells to find the target and replace
                for (int col = 1; col <= usedRange.Columns.Count; col++)
                {
                    Range cell = headerRange.Cells[1, col];
                    string headerValue = cell.Value2?.ToString();

                    if (headerValue == targetValue)
                    {
                        cell.Value2 = replaceValue;
                        break; // Stop after finding and replacing (assuming unique header names)
                    }
                }


                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);

                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = Path.Combine(outputFolder, fname + extension); // Use Path.Combine
                workbook.SaveAs(csvFilePath);

            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
            finally
            {
                workbook.Close(SaveChanges: false); // Important to prevent double saving
                excelApp.Quit();

                ReleaseObject(worksheet);
                ReleaseObject(workbook);
                ReleaseObject(excelApp);

                methodCount++;
            }
        }

        public static void FindCellValueAndReplaceInColumn(string filePath)
        {
            // Create an instance of Excel Application
            Application excelApp = new Application();
            excelApp.DisplayAlerts = false;

            // Open the Excel workbook
            Workbook workbook = excelApp.Workbooks.Open(filePath);

            // Get the worksheet
            Worksheet worksheet = (Worksheet)workbook.Sheets[1];

            try
            {
                string[] arr = args.Split('#');
                int columnIndex = int.Parse(arr[0]);
                string targetValue = arr[1];
                string replaceValue = arr[2];


                // Get the used range of the worksheet
                Range usedRange = worksheet.UsedRange;

                // Get the values of the entire column
                Range columnRange = usedRange.Columns[columnIndex];

                // Get the cell values as an array
                object[,] values = (object[,])columnRange.Value;

                // Loop through each cell in the column
                for (int i = 1; i <= values.GetLength(0); i++)
                {
                    // Check if the cell value matches the target value
                    if (values[i, 1] != null && values[i, 1].ToString() == targetValue)
                    {
                        // Replace the target value with the replace value
                        values[i, 1] = replaceValue;
                    }
                }

                // Update the column range with the new values
                columnRange.Value = values;

                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);

                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;
                workbook.SaveAs(csvFilePath);

            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
            finally
            {
                workbook.Close();

                // Quit Excel application
                excelApp.Quit();

                // Clean up
                ReleaseObject(worksheet);
                ReleaseObject(workbook);
                ReleaseObject(excelApp);
                methodCount++;
            }
        }

        public static void FindAndReplaceCharacterInColumn(string filePath)
        {
            string[] arr = args.Split('#');
            int columnIndex;
            string targetCharacter, replaceValue;
            Application excelApp = null;
            Workbook workbook = null;
            Worksheet worksheet = null;

            try
            {
                columnIndex = int.Parse(arr[0]);
                targetCharacter = arr[1];
                replaceValue = arr[2];
            }
            catch (Exception ex)
            {
                LogWithRed("Error parsing arguments: " + ex.Message);
                CreateLogs("Error parsing arguments: " + ex.Message, logFilePath);
                return;
            }

            try
            {
                // Create an instance of Excel Application
                excelApp = new Application();
                excelApp.DisplayAlerts = false;

                // Open the Excel workbook
                workbook = excelApp.Workbooks.Open(filePath);

                // Get the worksheet
                worksheet = (Worksheet)workbook.Sheets[1];

                // Get the used range of the worksheet
                Range usedRange = worksheet.UsedRange;

                // Get the values of the entire column
                Range columnRange = usedRange.Columns[columnIndex];

                // Get the cell values as an array
                object[,] values = (object[,])columnRange.Value;

                // Loop through each cell in the column
                for (int i = 1; i <= values.GetLength(0); i++)
                {
                    if (values[i, 1] != null)
                    {
                        string cellValue = values[i, 1].ToString();
                        if (cellValue.Contains(targetCharacter))
                        {
                            // Replace only the target characters within the cell value
                            cellValue = cellValue.Replace(targetCharacter, replaceValue);
                            values[i, 1] = cellValue;
                        }
                    }
                }

                // Update the column range with the new values
                columnRange.Value = values;

                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);

                if (!string.IsNullOrEmpty(newfilename))
                {
                    fname = newfilename;
                }

                string csvFilePath = Path.Combine(outputFolder, fname + extension);
                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error processing Excel file: " + ex.Message);
                CreateLogs("Error processing Excel file: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                if (workbook != null)
                {
                    workbook.Close(false);
                    ReleaseObject(workbook);
                }
                if (excelApp != null)
                {
                    excelApp.Quit();
                    ReleaseObject(excelApp);
                }
                ReleaseObject(worksheet);
            }
        }


        static void ConvertExcelToCSV(string excelFilePath)
        {
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string fname = Path.GetFileNameWithoutExtension(excelFilePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }
                string csvFilePath = outputFolder + @"\" + fname + ".csv";
                excelApp = new Excel.Application();
                excelApp.DisplayAlerts = false;
                workbook = excelApp.Workbooks.Open(excelFilePath);
                worksheet = (Excel.Worksheet)workbook.Sheets[1];

                if (File.Exists(csvFilePath))
                {
                    // If it does, delete the existing CSV file
                    File.Delete(csvFilePath);
                }
                // Save the worksheet as CSV
                worksheet.SaveAs(csvFilePath, Excel.XlFileFormat.xlCSV);

                Console.WriteLine("Conversion to CSV completed for " + fname);
                CreateLogs("Conversion to CSV completed for " + fname, logFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                // Close and release resources
                workbook.Close(false);
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }
        static void changeextension(string inputFilePath)
        {
            try
            {
                if (File.Exists(inputFilePath))
                {
                    //string fname = Path.GetFileName(inputFilePath);
                    //string csvFilePath = outputFolder + @"\" + fname;
                    string fname = Path.GetFileNameWithoutExtension(inputFilePath);
                    string extension = Path.GetExtension(inputFilePath);

                    if (newfilename != "")
                    {
                        fname = newfilename;
                    }
                    string csvFilePath = outputFolder + @"\" + fname;
                    FileInfo f = new FileInfo(inputFilePath);
                    f.CopyTo(Path.ChangeExtension(csvFilePath, changeextensionto));
                }
                else
                {
                    Console.WriteLine("Error: The specified input file does not exist.");
                    CreateLogs("Error: The specified input file does not exist.", logFilePath);
                }
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
        }
        //static void Copyfile(string inputFilePath)
        //{
        //    try
        //    {
        //        if (File.Exists(inputFilePath))
        //        {
        //            string fname = Path.GetFileNameWithoutExtension(inputFilePath);
        //            string extension = Path.GetExtension(inputFilePath);
        //            if (newfilename != "")
        //            {
        //                fname = newfilename;
        //            }

        //            string csvFilePath = outputFolder + @"\" + fname + extension;
        //            FileInfo f = new FileInfo(inputFilePath);
        //            if (!File.Exists(csvFilePath))
        //            {
        //                f.CopyTo(csvFilePath);
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine("Error: The specified input file does not exist.");
        //            // Log("Error: The specified input file does not exist."); // Uncomment if you have a logging method
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error: " + ex.Message);
        //        // Log("Error: " + ex.Message); // Uncomment if you have a logging method
        //    }
        //}


        public static void Copyfile(string inputFilePath)
        {
            try
            {
                if (File.Exists(inputFilePath))
                {
                    string fname = Path.GetFileNameWithoutExtension(inputFilePath);
                    string extension = Path.GetExtension(inputFilePath);

                    if (!string.IsNullOrEmpty(newfilename))
                    {
                        fname = newfilename;
                    }

                    string csvFilePath = Path.Combine(outputFolder, fname + extension);

                    File.Copy(inputFilePath, csvFilePath, true); // Overwrite if the file exists
                    Console.WriteLine($"File copied successfully to {csvFilePath}");
                    CreateLogs($"File copied successfully to {csvFilePath}", logFilePath);
                }
                else
                {
                    Console.WriteLine("Error: The specified input file does not exist.");
                    CreateLogs("Error: The specified input file does not exist.", logFilePath);
                }
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
        }


        public static void CutAndPasteFile(string inputFilePath)
        {
            try
            {
                if (File.Exists(inputFilePath))
                {
                    string fname = Path.GetFileNameWithoutExtension(inputFilePath);
                    string extension = Path.GetExtension(inputFilePath);

                    if (!string.IsNullOrEmpty(newfilename))
                    {
                        fname = newfilename;
                    }

                    string destinationFilePath = Path.Combine(outputFolder, fname + extension);

                    if (File.Exists(destinationFilePath))
                    {
                        File.Delete(destinationFilePath); // Delete the file if it already exists
                    }

                    File.Move(inputFilePath, destinationFilePath);
                    Console.WriteLine($"File moved successfully to {destinationFilePath}");
                    CreateLogs($"File moved successfully to {destinationFilePath}", logFilePath); // Uncomment if you have a logging method
                }
                else
                {
                    Console.WriteLine("Error: The specified input file does not exist.");
                    CreateLogs("Error: The specified input file does not exist.", logFilePath); // Uncomment if you have a logging method
                }
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath); // Uncomment if you have a logging method
            }
        }
        static void ConvertFileToText(string inputFilePath)
        {
            try
            {
                string fname = Path.GetFileNameWithoutExtension(inputFilePath);
                string outputfolder = outputFolder + @"\" + fname + ".txt";
                // Check if the input file exists
                if (File.Exists(inputFilePath))
                {
                    // Read all text from the input file
                    string fileContent = File.ReadAllText(inputFilePath);

                    // Write the content to the output text file
                    File.WriteAllText(outputFolder, fileContent);

                    Console.WriteLine("Conversion completed. Text file saved at: " + outputfolder);
                    CreateLogs("Conversion completed. Text file saved at: " + outputfolder, logFilePath);
                }
                else
                {
                    Console.WriteLine("Error: The specified input file does not exist.");
                    CreateLogs("Error: The specified input file does not exist.", logFilePath);
                }
            }
            catch (Exception ex)
            {
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
        }

        static void ConvertCommaToColumns(string filePath)
        {
            Excel.Application excelApp = new Excel.Application();
            excelApp.DisplayAlerts = false;
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet worksheet = workbook.ActiveSheet;

            string csvFilePath = "";
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fname = Path.GetFileNameWithoutExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }
            csvFilePath = outputFolder + @"\" + fname + ".csv";

            try
            {

                Excel.Range range = worksheet.UsedRange;
                range.TextToColumns(Destination: range, DataType: Excel.XlTextParsingType.xlDelimited,
                                     TextQualifier: Excel.XlTextQualifier.xlTextQualifierDoubleQuote,
                                     ConsecutiveDelimiter: false, Tab: false, Semicolon: false,
                                     Comma: true, Space: false, Other: false, OtherChar: ",", FieldInfo: null);

                Console.WriteLine("Text converted to columns separated by commas successfully.");
                CreateLogs("Text converted to columns separated by commas successfully.", logFilePath);
                worksheet.SaveAs(csvFilePath, Excel.XlFileFormat.xlWorkbookNormal);

            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("An error occurred: " + ex.Message);
                CreateLogs("An error occurred: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();

                ReleaseObject(worksheet);
                ReleaseObject(workbook);
                ReleaseObject(excelApp);
            }
        }

        static void ConvertPipeToColumns(string filePath)
        {
            Excel.Application excelApp = new Excel.Application();
            excelApp.DisplayAlerts = false;
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet worksheet = workbook.ActiveSheet;

            string csvFilePath = "";
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fname = Path.GetFileNameWithoutExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }
            csvFilePath = outputFolder + @"\" + fname + ".csv";

            try
            {
                Excel.Range range = worksheet.UsedRange;
                range.TextToColumns(Destination: range, DataType: Excel.XlTextParsingType.xlDelimited,
                                    TextQualifier: Excel.XlTextQualifier.xlTextQualifierDoubleQuote,
                                    ConsecutiveDelimiter: false, Tab: false, Semicolon: false,
                                    Comma: false, Space: false, Other: true, OtherChar: "|");

                Console.WriteLine("Text converted to columns separated by pipe '|' successfully.");
                CreateLogs("Text converted to columns separated by pipe '|' successfully.", logFilePath);

                // Save as CSV file
                worksheet.SaveAs(csvFilePath, Excel.XlFileFormat.xlCSV);

            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("An error occurred: " + ex.Message);
                CreateLogs("An error occurred: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();

                ReleaseObject(worksheet);
                ReleaseObject(workbook);
                ReleaseObject(excelApp);
            }
        }
        static void ConvertexttoCol(string excelFilePath)
        {
            string csvFilePath = "";
            // ClearFormatOfSheet(excelFilePath);
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;
            string[] arr = args.Split('#');
            string delimeter = arr[0];
            try
            {
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string fname = Path.GetFileNameWithoutExtension(excelFilePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }
                csvFilePath = outputFolder + @"\" + fname + ".csv";
                excelApp = new Excel.Application();
                excelApp.DisplayAlerts = false;

                workbook = excelApp.Workbooks.Open(excelFilePath);
                ClearFormatworkbook(excelFilePath, workbook);

                worksheet = (Excel.Worksheet)workbook.Sheets[1];

                if (File.Exists(csvFilePath))
                {
                    // If it does, delete the existing CSV file
                    File.Delete(csvFilePath);
                }
                // Save the worksheet as CSV
                // worksheet.SaveAs(csvFilePath, Excel.XlFileFormat.xlCSV);
                // Specify the range to apply Text to Columns
                Excel.Range range = worksheet.UsedRange;

                // Specify delimiter and other options
                Excel.Range textToColumnsRange = range;
                //  Excel.XlTextParsingType dataType = Excel.XlTextParsingType.xlDelimited;
                //Excel.XlTextQualifier textQualifier = Excel.XlTextQualifier.xlTextQualifierNone;
                // char delimiter = '|'; // Change this to your desired delimiter

                //           // Second operation with "\t" (tab)
                //           textToColumnsRange.TextToColumns(textToColumnsRange, Excel.XlTextParsingType.xlDelimited,
                //               Excel.XlTextQualifier.xlTextQualifierNone, false, false, false, false, false, false, false,
                //               Excel.XlColumnDataType.xlTextFormat, "\t");


                //                textToColumnsRange.TextToColumns(
                //    Destination: textToColumnsRange,
                //    Excel.XlTextParsingType.xlDelimited,
                //    TextQualifier: Excel.XlTextQualifier.xlTextQualifierNone,
                //    ConsecutiveDelimiter: true,
                //    Tab: true,
                //    Semicolon: false,
                //    Comma: false,
                //    Space: false,
                //    Other: true,
                //    OtherChar: "|",
                //    FieldInfo: null,
                //    Excel.XlColumnDataType.xlGeneralFormat
                //);
                excelApp.DisplayAlerts = false;
                textToColumnsRange.TextToColumns(
    textToColumnsRange, // Destination
    Excel.XlTextParsingType.xlDelimited, // DataType
    Excel.XlTextQualifier.xlTextQualifierNone, // TextQualifier
    false, // ConsecutiveDelimiter
    true, // Tab
    false, // Semicolon
    false, // Comma
    false, // Space
    true, // Other
    delimeter,   // OtherChar
    Excel.XlColumnDataType.xlGeneralFormat, // FieldInfo
    Type.Missing, // DecimalSeparator
    Type.Missing, // ThousandsSeparator
    Type.Missing  // TrailingMinusNumbers
);
                worksheet.SaveAs(csvFilePath, Excel.XlFileFormat.xlWorkbookNormal);

                Console.WriteLine("Conversion text to column completed for " + fname);
                CreateLogs("Conversion text to column completed for " + fname, logFilePath);

            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                // Close and release resources
                workbook.Close(false);
                excelApp.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
            ClearFormat(csvFilePath);
        }


        static void ConvertExcelToxls(string excelFilePath)
        {
            Excel.Workbook workbook = null;
            Excel.Application excelApp = null;
            Excel.Worksheet worksheet = null;

            try
            {

                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string fname = Path.GetFileNameWithoutExtension(excelFilePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }
                string csvFilePath = outputFolder + @"\" + fname + ".xls";
                excelApp = new Excel.Application();
                excelApp.DisplayAlerts = false;
                workbook = excelApp.Workbooks.Open(excelFilePath);
                worksheet = (Excel.Worksheet)workbook.Sheets[1];
                if (File.Exists(csvFilePath))
                {
                    // If it does, delete the existing CSV file
                    File.Delete(csvFilePath);
                }
                // Save the worksheet as CSV
                worksheet.SaveAs(csvFilePath, Excel.XlFileFormat.xlWorkbookNormal);
                Console.WriteLine("Conversion to XLS completed for " + fname);
                CreateLogs("Conversion to XLS completed for " + fname, logFilePath);

            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                // Close and release resources
                workbook.Close(false);
                excelApp.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }

        }

        public static void ConvertXmlToXls(string xmlFilePath)
        {
            Excel.Workbook workbook = null;
            Excel.Application excelApp = null;
            Excel.Worksheet worksheet = null;


            try
            {
                string fname = Path.GetFileNameWithoutExtension(xmlFilePath);
                if (!string.IsNullOrEmpty(newfilename))
                {
                    fname = newfilename;
                }
                string xlsFilePath = Path.Combine(outputFolder, fname + ".xls");

                excelApp = new Excel.Application();
                excelApp.DisplayAlerts = false;

                // Open the XML file as a workbook
                workbook = excelApp.Workbooks.Open(xmlFilePath, Format: Excel.XlFileFormat.xlXMLSpreadsheet);

                worksheet = (Excel.Worksheet)workbook.Sheets[1];

                if (File.Exists(xlsFilePath))
                {
                    File.Delete(xlsFilePath);
                }

                // Save the worksheet as XLS
                workbook.SaveAs(xlsFilePath, Excel.XlFileFormat.xlWorkbookNormal);
                Console.WriteLine("Conversion to XLS completed for " + fname);
                CreateLogs("Conversion to XLS completed for " + fname, logFilePath);

            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                // Close and release resources
                if (workbook != null)
                {
                    workbook.Close(false);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                }

                if (excelApp != null)
                {
                    excelApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                }

                if (worksheet != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                }
            }
        }
        static void IterateFilesInFolder(string folderPath, Dictionary<string, List<Action<string>>> keywordFunctions, string outputFolder)
        {
            int filecount = 0;
            // Check if the directory exists
            if (Directory.Exists(folderPath))
            {
                // Get all files in the directory
                string[] files = Directory.GetFiles(folderPath);
                bool runMethod = false;
                // Iterate over each file
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    string filepath = Path.GetFullPath(file);
                    if (changename.ToLower().Trim() == "true")
                    {
                        foreach (var kvp in nameconfig)
                        {
                            // Check if the keyword exists in the file name
                            if (fileName.ToLower().Contains(kvp.Key.ToLower()))
                            {
                                // If a match is found, call the associated function
                                newfilename = kvp.Value;
                            }
                        }

                    }

                    // Check if any of the keywords exist in the file name
                    foreach (var kvp in keywordFunctions)
                    {
                        // Check if the keyword exists in the file name
                        if (fileName.ToLower().Contains(kvp.Key.ToLower()))
                        {
                            // If a match is found, call the associated function
                            for (int i = 0; i < kvp.Value.Count; i++)
                            {
                                kvp.Value[i](file);
                                if (kvp.Value[i].Method.Name.Equals("MergeMultipleFiles") || kvp.Value[i].Method.Name.Equals("RenameFilesCondition"))
                                    runMethod = true;
                            }
                            filecount++;
                        }
                    }
                    if (runMethod)
                        break;
                }
            }
            else
            {
                Console.WriteLine("Directory does not exist.");
                CreateLogs("Directory does not exist.", logFilePath);
            }

            methodCount1++;
            Console.WriteLine("Total Files Converted " + filecount);
            CreateLogs("Total Files Converted " + filecount, logFilePath);

        }

        public static void ConvertColValueToNumber(string inputFilePath)
        {
            string fname = newfilename != "" ? newfilename : Path.GetFileNameWithoutExtension(inputFilePath);
            string csvFilePath = Path.Combine(outputFolder, $"{fname}.csv");

            string[] lines = File.ReadAllLines(inputFilePath);

            for (int i = 1; i < lines.Length; i++)
            {
                string[] columns = lines[i].Split(',');

                for (int j = 0; j < columns.Length; j++)
                {
                    string cellValue = columns[j].Trim();

                    if (double.TryParse(cellValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double numericValue))
                    {
                        columns[j] = numericValue.ToString("G", CultureInfo.InvariantCulture); // Preserve decimal precision
                    }
                    else { columns[j] = cellValue; }
                }

                // Reconstruct the processed line
                lines[i] = string.Join(",", columns);
            }

            // Write back to a new CSV file
            File.WriteAllLines(csvFilePath, lines);
        }

        public static void UpdatedConvertColValuetoNumber(string filePath)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = null;
            excelApp.DisplayAlerts = false;
            string[] columnArgs = args.Split('#');

            try
            {
                workbook = excelApp.Workbooks.Open(filePath);

                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (!string.IsNullOrEmpty(newfilename))
                {
                    fname = newfilename;
                }

                string csvFilePath = Path.Combine(outputFolder, fname + extension);

                Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1];

                foreach (string columnArg in columnArgs)
                {
                    if (int.TryParse(columnArg, out int columnNumber) && columnNumber > 0)
                    {
                        // Get the range for the current column
                        Excel.Range columnRange = worksheet.Columns[columnNumber];

                        // Read all values in one go
                        object[,] columnValues = columnRange.Value2;

                        // Process the values in memory
                        int rowCount = columnValues.GetLength(0);
                        for (int row = 1; row <= rowCount; row++)
                        {
                            if (columnValues[row, 1] != null)
                            {
                                string cellValue = columnValues[row, 1].ToString();

                                // Convert text to number, ensuring precision
                                if (decimal.TryParse(cellValue, out decimal numericValue))
                                {
                                    columnValues[row, 1] = numericValue; // Assign as numeric value
                                }
                            }
                        }

                        // Write the updated values back to the range in one go
                        columnRange.Value2 = columnValues;

                        // Force Excel to recalculate the range to recognize numbers
                        columnRange.NumberFormat = "0";
                        columnRange.Value2 = columnRange.Value2; // Reassign to refresh formatting
                    }
                }

                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                // Optionally log the error
            }
            finally
            {
                workbook.Close(false);
                excelApp.Quit();
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }
        public static void UpdatedConvertColValuetoNumber_V2(string filePath)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = null;
            excelApp.DisplayAlerts = false;
            string[] columnArgs = args.Split('#');

            try
            {
                workbook = excelApp.Workbooks.Open(filePath);

                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (!string.IsNullOrEmpty(newfilename))
                {
                    fname = newfilename;
                }

                string csvFilePath = Path.Combine(outputFolder, fname + extension);

                Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1];

                foreach (string columnArg in columnArgs)
                {
                    if (int.TryParse(columnArg, out int columnNumber) && columnNumber > 0)
                    {
                        // Get the range for the current column
                        Excel.Range columnRange = worksheet.Columns[columnNumber];

                        // Read all values in one go
                        object[,] columnValues = columnRange.Value2;

                        // Process the values in memory
                        int rowCount = columnValues.GetLength(0);
                        for (int row = 1; row <= rowCount; row++)
                        {
                            if (columnValues[row, 1] != null)
                            {
                                string cellValue = columnValues[row, 1].ToString();

                                // Convert text to number, ensuring precision
                                if (decimal.TryParse(cellValue, out decimal numericValue))
                                {
                                    columnValues[row, 1] = numericValue;
                                }
                            }
                        }

                        // Write the updated values back to the range in one go
                        //columnRange.Value2 = columnValues;

                        //// Force Excel to recalculate the range to recognize numbers
                        //columnRange.NumberFormat = "0";
                        //columnRange.Value2 = columnRange.Value2; // Reassign to refresh formatting
                        // Assign updated values back to Excel
                        columnRange.Value2 = columnValues;

                        // Set NumberFormat to "General" to avoid unnecessary decimal places
                        columnRange.NumberFormat = "0.00";
                    }
                }

                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                // Optionally log the error
            }
            finally
            {
                workbook.Close(false);
                excelApp.Quit();
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        public static string[] FindFilesWithPartialName(string directoryPath, string partialFileName)
        {
            try
            {
                // Check if the directory exists
                if (!Directory.Exists(directoryPath))
                {
                    throw new DirectoryNotFoundException($"Directory '{directoryPath}' not found.");
                }

                // Get all files in the directory matching the partial file name
                string[] matchingFiles = Directory.GetFiles(directoryPath)
                    .Where(file => Path.GetFileName(file).Contains(partialFileName))
                    .ToArray();

                return matchingFiles;
            }
            catch (Exception ex)
            {
                LogWithRed($"An error occurred: {ex.Message}");
                CreateLogs($"An error occurred: {ex.Message}", logFilePath);
                return new string[0]; // Return an empty array if an error occurs
            }
        }



        public static void AppendDataToFile(string FilePath)
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fname = Path.GetFileNameWithoutExtension(FilePath);
            string extension = Path.GetExtension(FilePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(FilePath);

            string mainFilePath = FilePath;
            string partialFilePath = currentDirectory;
            string[] arr = args.Split('#');
            if (!arr[0].Equals("NA"))
            {
                partialFilePath = currentDirectory + arr[0];
            }

            string partialFileName = arr[1];
            int startRow = int.Parse(arr[2]);
            string[] matchingFiles = FindFilesWithPartialName(partialFilePath, partialFileName);
            string otherFilePath = matchingFiles[0];
            // Ensure both files exist
            if (!File.Exists(mainFilePath))
            {
                Console.WriteLine("Main file does not exist.");
                CreateLogs("Main file does not exist.", logFilePath);
                return;
            }

            if (!File.Exists(otherFilePath))
            {
                Console.WriteLine("Other file does not exist.");
                CreateLogs("Other file does not exist.", logFilePath);
                return;
            }

            // Create Excel Application instance
            Excel.Application excelApp = new Excel.Application();

            // Disable alerts and screen updating to improve performance
            excelApp.DisplayAlerts = false;
            // excelApp.ScreenUpdating = false;

            // Open main workbook
            Excel.Workbook mainWorkbook = excelApp.Workbooks.Open(mainFilePath);
            // Open other workbook
            Excel.Workbook otherWorkbook = excelApp.Workbooks.Open(otherFilePath);

            // Get main worksheet
            Excel.Worksheet mainSheet = mainWorkbook.Sheets[1];

            // Get other worksheet
            Excel.Worksheet otherSheet = otherWorkbook.Sheets[1];

            try
            {


                // Get last row in other worksheet
                int lastRow = otherSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;

                // Iterate through rows in other file starting from startRow
                for (int rowIndex = startRow; rowIndex <= lastRow; rowIndex++)
                {
                    // Check if the row is empty
                    if (otherSheet.Cells[rowIndex, 1].Value != null)
                    {
                        // Get row from other worksheet
                        Excel.Range otherRow = otherSheet.Rows[rowIndex];

                        // Copy row to main worksheet
                        otherRow.Copy(mainSheet.Rows[mainSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row + 1]);
                    }
                }
                // Save changes to main workbook
                mainWorkbook.SaveAs(csvFilePath);

                // Close workbooks
                mainWorkbook.Close();
                otherWorkbook.Close();

                // Close Excel Application
                excelApp.Quit();

                Console.WriteLine("Data appended to main file at: " + csvFilePath);
                CreateLogs("Data appended to main file at: " + csvFilePath, logFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("An error occurred: " + ex.Message);
                CreateLogs("An error occurred: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                if (mainWorkbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(mainWorkbook);
                if (mainSheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(mainSheet);

                if (otherWorkbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(otherWorkbook);
                if (otherSheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(otherSheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

            }

        }

        public static void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                LogWithRed("An error occurred while releasing object: " + ex.Message);
                CreateLogs("An error occurred while releasing object: " + ex.Message, logFilePath);
            }
            finally
            {
                GC.Collect();
            }
        }


        public static void FilterAndDeleteRows(string filePath)
        {
            try
            {
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;
                string fileName = Path.GetFileName(filePath);
                //outputFolder += "\\" + fileName;
                //string outputFolder = Path.Combine(Path.GetDirectoryName(filePath), "Output_Files");
                //string outputFile = Path.Combine(outputFolder, fileName);

                // Load Excel application
                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
                Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[1]; // Assuming the first sheet

                excelApp.DisplayAlerts = false;
                // Extract filter criteria
                string[] arr = args.Split('#');
                int columnIndex = int.Parse(arr[0]);
                string searchText = arr[1];
                string headerText = arr[2];

                // Filter and delete rows
                Excel.Range usedRange = sheet.UsedRange;
                int rowCount = usedRange.Rows.Count;
                int startRowIndex = 1; // Assuming the first row is header

                for (int rowIndex = startRowIndex; rowIndex <= rowCount; rowIndex++)
                {
                    Excel.Range cell = (Excel.Range)sheet.Cells[rowIndex, columnIndex];
                    string cellContent = cell.Value != null ? cell.Value.ToString() : "";
                    if (!(cellContent.Equals(searchText) || cellContent.Equals(headerText)))
                    {
                        Excel.Range rowToDelete = (Excel.Range)sheet.Rows[rowIndex];
                        rowToDelete.Delete();
                        rowIndex--; // Adjust rowIndex since the next row moves up after deletion
                        rowCount--; // Adjust rowCount after deletion
                    }
                }

                // Save changes
                workbook.SaveAs(csvFilePath);
                workbook.Close();
                excelApp.Quit();
                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (sheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);


                Console.WriteLine("Rows not containing the specified text in the specified column have been deleted.");
                CreateLogs("Rows not containing the specified text in the specified column have been deleted.", logFilePath);
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
        }

        public static void FilterAndReplaceInOtherColumn(string filePath)
        {
            try
            {

                int filterColumnNumber;
                object targetValue;
                int replaceColumnNumber;
                string replacementValue;
                if (SplitParameter != "")
                {
                    char[] charArray = SplitParameter.ToCharArray();
                    string[] arr2 = args.Split(charArray[0]);
                    filterColumnNumber = int.Parse(arr2[0]);
                    targetValue = arr2[1];
                    replaceColumnNumber = int.Parse(arr2[2]);
                    replacementValue = arr2[3];

                }
                else
                {
                    string[] arr = args.Split('#');
                    filterColumnNumber = int.Parse(arr[0]);
                    targetValue = arr[1];
                    replaceColumnNumber = int.Parse(arr[2]);
                    replacementValue = arr[3];
                }

                Excel.Application excelApp = new Excel.Application();
                excelApp.DisplayAlerts = false;
                Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
                Excel.Worksheet worksheet = workbook.Sheets[1];
                Excel.Range usedRange = worksheet.UsedRange;
                Excel.Range filterColumn = usedRange.Columns[filterColumnNumber];
                Excel.Range replaceColumn = usedRange.Columns[replaceColumnNumber];

                if (filterColumn != null && replaceColumn != null)
                {
                    // Convert targetValue to string
                    string targetValueString = Convert.ToString(targetValue);

                    // Loop through each cell in the filter column
                    foreach (Excel.Range cell in filterColumn.Cells)
                    {
                        if (cell.Value2 != null && cell.Value2.ToString().Trim() == targetValueString)
                        {
                            // Get the corresponding cell in the replace column and replace its value
                            Excel.Range replaceCell = (Excel.Range)replaceColumn.Cells[cell.Row - filterColumn.Row + 1];
                            replaceCell.Value = replacementValue;
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Filter column number '{filterColumnNumber}' or replace column number '{replaceColumnNumber}' is invalid.");
                    CreateLogs($"Filter column number '{filterColumnNumber}' or replace column number '{replaceColumnNumber}' is invalid.", logFilePath);
                }


                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;

                // Save and close Excel
                workbook.SaveAs(csvFilePath);
                workbook.Close();
                excelApp.Quit();

                ReleaseObject(worksheet);
                ReleaseObject(workbook);
                ReleaseObject(excelApp);
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
        }

        public static void ConvertCsvTxtToCsv(string sourceFilePath)
        {
            try
            {
                // Check if the source file exists
                if (!File.Exists(sourceFilePath))
                {
                    Console.WriteLine("Source file does not exist.");
                    return;
                }

                // Check if the file has the correct extension
                if (!sourceFilePath.EndsWith(".csv.txt"))
                {
                    Console.WriteLine("The source file is not a '.csv.txt' file.");
                    return;
                }

                // Create the target file path by replacing the extension
                string targetFilePath = sourceFilePath.Replace(".csv.txt", ".csv");

                // Rename the file by copying and then deleting the original
                File.Move(sourceFilePath, targetFilePath);

                Console.WriteLine($"File successfully converted to: {targetFilePath}");
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed($"An error occurred during file conversion: {ex.Message}");
            }
        }

        public static void FilterAndReplaceInOtherColumnIfContains(string filePath)
        {
            try
            {
                int filterColumnNumber;
                object targetValue;
                int replaceColumnNumber;
                string replacementValue;
                if (SplitParameter != "")
                {
                    char[] charArray = SplitParameter.ToCharArray();
                    string[] arr2 = args.Split(charArray[0]);
                    filterColumnNumber = int.Parse(arr2[0]);
                    targetValue = arr2[1];
                    replaceColumnNumber = int.Parse(arr2[2]);
                    replacementValue = arr2[3];

                }
                else
                {
                    string[] arr = args.Split('#');
                    filterColumnNumber = int.Parse(arr[0]);
                    targetValue = arr[1];
                    replaceColumnNumber = int.Parse(arr[2]);
                    replacementValue = arr[3];
                }

                Excel.Application excelApp = new Excel.Application();
                excelApp.DisplayAlerts = false;
                Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
                Excel.Worksheet worksheet = workbook.Sheets[1];
                Excel.Range usedRange = worksheet.UsedRange;
                Excel.Range filterColumn = usedRange.Columns[filterColumnNumber];
                Excel.Range replaceColumn = usedRange.Columns[replaceColumnNumber];

                if (filterColumn != null && replaceColumn != null)
                {
                    // Convert targetValue to string
                    string targetValueString = Convert.ToString(targetValue);

                    // Loop through each cell in the filter column
                    foreach (Excel.Range cell in filterColumn.Cells)
                    {
                        if (cell.Value2 != null && cell.Value2.ToString().Trim().Contains(targetValueString))
                        {
                            // Get the corresponding cell in the replace column and replace its value
                            Excel.Range replaceCell = (Excel.Range)replaceColumn.Cells[cell.Row - filterColumn.Row + 1];
                            replaceCell.Value = replacementValue;
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Filter column number '{filterColumnNumber}' or replace column number '{replaceColumnNumber}' is invalid.");
                    CreateLogs($"Filter column number '{filterColumnNumber}' or replace column number '{replaceColumnNumber}' is invalid.", logFilePath);
                }


                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;

                // Save and close Excel
                workbook.SaveAs(csvFilePath);
                workbook.Close();
                excelApp.Quit();

                ReleaseObject(worksheet);
                ReleaseObject(workbook);
                ReleaseObject(excelApp);
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
        }

        public static void FilterAndCopyPasteInOtherColumn(string filePath)
        {


            int filterColumnNumber;
            string targetValue;
            int sourceColumnNumber;
            int targetColumnNumber;
            int startFilterRow;

            if (SplitParameter != "")
            {
                char[] charArray = SplitParameter.ToCharArray();
                string[] arr2 = args.Split(charArray[0]);
                filterColumnNumber = int.Parse(arr2[0]);
                targetValue = arr2[1];
                sourceColumnNumber = int.Parse(arr2[2]);
                targetColumnNumber = int.Parse(arr2[3]);
                startFilterRow = int.Parse(arr2[4]);

            }
            else
            {
                string[] arr = args.Split('#');
                filterColumnNumber = int.Parse(arr[0]);
                targetValue = arr[1];
                sourceColumnNumber = int.Parse(arr[2]);
                targetColumnNumber = int.Parse(arr[3]);
                startFilterRow = int.Parse(arr[4]);
            }



            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                excelApp = new Excel.Application();
                excelApp.DisplayAlerts = false;
                workbook = excelApp.Workbooks.Open(filePath);
                worksheet = (Excel.Worksheet)workbook.Sheets[1];
                Excel.Range usedRange = worksheet.UsedRange;

                Excel.Range filterColumn = usedRange.Columns[filterColumnNumber];
                Excel.Range sourceColumn = usedRange.Columns[sourceColumnNumber];
                Excel.Range targetColumn = usedRange.Columns[targetColumnNumber];

                if (filterColumn != null && sourceColumn != null && targetColumn != null)
                {
                    // Loop through each cell in the filter column starting from the specified row
                    for (int row = startFilterRow; row <= usedRange.Rows.Count; row++)
                    {
                        Excel.Range cell = (Excel.Range)filterColumn.Cells[row, 1];
                        if (cell.Value2 != null && cell.Value2.ToString().Trim().Contains(targetValue))
                        {
                            Excel.Range sourceCell = (Excel.Range)sourceColumn.Cells[row, 1];
                            Excel.Range targetCell = (Excel.Range)targetColumn.Cells[row, 1];
                            targetCell.Value = sourceCell.Value;
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Filter column number '{filterColumnNumber}', source column number '{sourceColumnNumber}', or target column number '{targetColumnNumber}' is invalid.");
                    CreateLogs($"Filter column number '{filterColumnNumber}', source column number '{sourceColumnNumber}', or target column number '{targetColumnNumber}' is invalid.", logFilePath);
                }

                string fname = string.IsNullOrEmpty(newfilename) ? Path.GetFileNameWithoutExtension(filePath) : newfilename;
                string extension = Path.GetExtension(filePath);

                if (string.IsNullOrEmpty(outputFolder))
                {
                    outputFolder = Path.Combine(Path.GetDirectoryName(filePath), "Output_Files");
                }

                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

                string csvFilePath = Path.Combine(outputFolder, fname + extension);

                workbook.SaveAs(csvFilePath);
                workbook.Close();
                excelApp.Quit();
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                Console.WriteLine("Filter and replace operation completed.");
                CreateLogs("Filter and replace operation completed.", logFilePath);
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed($"An error occurred: {ex.Message}");
                CreateLogs($"An error occurred: {ex.Message}", logFilePath);
            }


            Console.WriteLine("Filter and replace operation completed.");
            CreateLogs("Filter and replace operation completed.", logFilePath);
        }



        public static void FilterAndDeleteRowsListStartFromRow(string filePath)
        {
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(filePath);
            //outputFolder += "\\" + fileName;
            //string outputFolder = Path.Combine(Path.GetDirectoryName(filePath), "Output_Files");
            //string outputFile = Path.Combine(outputFolder, fileName);
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[1];
            try
            {
                // Load Excel application
                // Assuming the first sheet
                char[] charArray;
                string[] arr;
                excelApp.DisplayAlerts = false;
                // Extract filter criteria
                if (!string.IsNullOrEmpty(SplitParameter))
                {
                    charArray = SplitParameter.ToCharArray();
                    arr = args.Split(charArray[0]);
                }
                else
                    arr = args.Split('#');
                int columnIndex = int.Parse(arr[0]);
                string searchText = arr[1];
                string[] searchTextArr = searchText.Split(';');
                List<string> searchTextList = searchTextArr.ToList();
                string headerText = arr[2];
                int startRowIndex = int.Parse(arr[3]);
                // Filter and delete rows
                Excel.Range usedRange = sheet.UsedRange;
                int rowCount = usedRange.Rows.Count;


                for (int rowIndex = startRowIndex; rowIndex <= rowCount; rowIndex++)
                {
                    Excel.Range cell = (Excel.Range)sheet.Cells[rowIndex, columnIndex];
                    string cellContent = cell.Value != null ? cell.Value.ToString() : "";
                    cellContent = cellContent.Trim();
                    if (!(searchTextList.Contains(cellContent) || cellContent.Equals(headerText)))
                    {
                        Excel.Range rowToDelete = (Excel.Range)sheet.Rows[rowIndex];
                        rowToDelete.Delete();
                        rowIndex--; // Adjust rowIndex since the next row moves up after deletion
                        rowCount--; // Adjust rowCount after deletion
                    }
                }

                // Save changes
                workbook.SaveAs(csvFilePath);

                Console.WriteLine("Rows not containing the specified text in the specified column have been deleted.");
                CreateLogs("Rows not containing the specified text in the specified column have been deleted.", logFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed($"An error occurred: {ex.Message}");
                CreateLogs($"An error occurred: {ex.Message}", logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close(false);
                excelApp.Quit();
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                if (sheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
            }
        }
        static bool SearchTextContainsItems(List<string> searchTextList, string searchText)
        {
            foreach (string item in searchTextList)
            {
                if (item == "")
                {
                    if (item == searchText)
                    {
                        return true;
                    }
                }
                else if (searchText.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }

        public static void FilterAndDeleteRowsListIfContainsStartFromRow(string filePath)
        {
            try
            {
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;
                string fileName = Path.GetFileName(filePath);
                //outputFolder += "\\" + fileName;
                //string outputFolder = Path.Combine(Path.GetDirectoryName(filePath), "Output_Files");
                //string outputFile = Path.Combine(outputFolder, fileName);

                // Load Excel application
                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
                Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[1]; // Assuming the first sheet

                excelApp.DisplayAlerts = false;
                // Extract filter criteria
                string[] arr = args.Split('#');
                int columnIndex = int.Parse(arr[0]);
                string searchText = arr[1];
                string[] searchTextArr = searchText.Split(';');
                List<string> searchTextList = searchTextArr.ToList();
                string headerText = arr[2];
                int startRowIndex = int.Parse(arr[3]);
                // Filter and delete rows
                Excel.Range usedRange = sheet.UsedRange;
                int rowCount = usedRange.Rows.Count;
                // Create a list to hold the rows to delete
                int lastRow = usedRange.Rows.Count;
                // Find the next available row for pasting (after the header)
                int pasteRow = startRowIndex; // Start pasting from the second row

                for (int rowIndex = startRowIndex; rowIndex <= rowCount; rowIndex++)
                {
                    Excel.Range cell = (Excel.Range)sheet.Cells[rowIndex, columnIndex];
                    string cellContent = cell.Value != null ? cell.Value.ToString() : "";
                    cellContent = cellContent.Trim();
                    if (!(!(SearchTextContainsItems(searchTextList, cellContent) || cellContent.Equals(headerText))))
                    {
                        //Excel.Range rowToDelete = (Excel.Range)sheet.Rows[rowIndex];
                        //rowToDelete.Delete();
                        //rowIndex--; // Adjust rowIndex since the next row moves up after deletion
                        //rowCount--; // Adjust rowCount after deletion
                        // Copy entire row to the new location
                        Excel.Range sourceRow = (Excel.Range)sheet.Rows[rowIndex];
                        Excel.Range destinationRow = (Excel.Range)sheet.Rows[pasteRow];
                        sourceRow.Copy(destinationRow);

                        // Increment pasteRow for the next row to paste
                        pasteRow++;
                    }
                }
                // Clear contents of rows below the last pasted row
                if (pasteRow <= lastRow)
                {
                    Excel.Range clearRange = sheet.Rows[pasteRow].Resize[lastRow - pasteRow + 1];
                    clearRange.ClearContents();
                }

                // Save changes
                workbook.SaveAs(csvFilePath);
                workbook.Close();
                excelApp.Quit();
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (sheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

                Console.WriteLine("Rows not containing the specified text in the specified column have been deleted.");
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
        }

        public static void FilterAndDeleteRowsListIfContainsStartFromRow_BACKUP(string filePath)
        {
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(filePath);
            //outputFolder += "\\" + fileName;
            //string outputFolder = Path.Combine(Path.GetDirectoryName(filePath), "Output_Files");
            //string outputFile = Path.Combine(outputFolder, fileName);

            // Load Excel application
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[1]; // Assuming the first sheet

            excelApp.DisplayAlerts = false;
            // Extract filter criteria
            string[] arr = args.Split('#');
            int columnIndex = int.Parse(arr[0]);
            string searchText = arr[1];
            string[] searchTextArr = searchText.Split(';');
            List<string> searchTextList = searchTextArr.ToList();
            string headerText = arr[2];
            int startRowIndex = int.Parse(arr[3]);
            // Filter and delete rows
            Excel.Range usedRange = sheet.UsedRange;
            int rowCount = usedRange.Rows.Count;


            for (int rowIndex = startRowIndex; rowIndex <= rowCount; rowIndex++)
            {
                Excel.Range cell = (Excel.Range)sheet.Cells[rowIndex, columnIndex];
                string cellContent = cell.Value != null ? cell.Value.ToString() : "";
                cellContent = cellContent.Trim();
                if (!(SearchTextContainsItems(searchTextList, cellContent) || cellContent.Equals(headerText)))
                {
                    Excel.Range rowToDelete = (Excel.Range)sheet.Rows[rowIndex];
                    rowToDelete.Delete();
                    rowIndex--; // Adjust rowIndex since the next row moves up after deletion
                    rowCount--; // Adjust rowCount after deletion
                }
            }

            // Save changes
            workbook.SaveAs(csvFilePath);
            workbook.Close();
            excelApp.Quit();
            if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            if (sheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
            if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            Console.WriteLine("Rows not containing the specified text in the specified column have been deleted.");
            CreateLogs("Rows not containing the specified text in the specified column have been deleted.", logFilePath);
        }
        public static void FilterAndDeleteRowsStartFromRow(string filePath)
        {
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(filePath);
            //outputFolder += "\\" + fileName;
            //string outputFolder = Path.Combine(Path.GetDirectoryName(filePath), "Output_Files");
            //string outputFile = Path.Combine(outputFolder, fileName);

            // Load Excel application
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[1]; // Assuming the first sheet

            excelApp.DisplayAlerts = false;
            // Extract filter criteria
            string[] arr = args.Split('#');
            int columnIndex = int.Parse(arr[0]);
            string searchText = arr[1];
            string headerText = arr[2];
            int startRowIndex = int.Parse(arr[3]);
            // Filter and delete rows
            Excel.Range usedRange = sheet.UsedRange;
            int rowCount = usedRange.Rows.Count;
            try
            {
                for (int rowIndex = startRowIndex; rowIndex <= rowCount; rowIndex++)
                {
                    Excel.Range cell = (Excel.Range)sheet.Cells[rowIndex, columnIndex];
                    string cellContent = cell.Value != null ? cell.Value.ToString() : "";
                    if (!(cellContent.Equals(searchText) || cellContent.Equals(headerText)))
                    {
                        Excel.Range rowToDelete = (Excel.Range)sheet.Rows[rowIndex];
                        rowToDelete.Delete();
                        rowIndex--; // Adjust rowIndex since the next row moves up after deletion
                        rowCount--; // Adjust rowCount after deletion
                    }
                }
                // Save changes
                workbook.SaveAs(csvFilePath);
                Console.WriteLine("Rows not containing the specified text in the specified column have been deleted.");
                CreateLogs("Rows not containing the specified text in the specified column have been deleted.", logFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();


                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (sheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }

        }


        public static void FilterAndDeleteUsingLastUsedRowStartFromRow(string filePath)
        {
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(filePath);
            //outputFolder += "\\" + fileName;
            //string outputFolder = Path.Combine(Path.GetDirectoryName(filePath), "Output_Files");
            //string outputFile = Path.Combine(outputFolder, fileName);

            // Load Excel application
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[1]; // Assuming the first sheet

            excelApp.DisplayAlerts = false;
            // Extract filter criteria
            string[] arr = args.Split('#');
            int columnIndex = int.Parse(arr[0]);
            string searchText = arr[1];
            string headerText = arr[2];
            int startRowIndex = int.Parse(arr[3]);
            // Filter and delete rows
            //Excel.Range usedRange = sheet.UsedRange;
            //int rowCount = usedRange.Rows.Count;
            Excel.Range lastCell = sheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            int lastRow = lastCell.Row;

            try
            {

                for (int rowIndex = startRowIndex; rowIndex <= lastRow; rowIndex++)
                {
                    Excel.Range cell = (Excel.Range)sheet.Cells[rowIndex, columnIndex];
                    string cellContent = cell.Value != null ? cell.Value.ToString() : "";
                    if (!(cellContent.Contains(searchText) || cellContent.Trim().Equals(headerText)))
                    {
                        Excel.Range rowToDelete = (Excel.Range)sheet.Rows[rowIndex];
                        rowToDelete.Delete();
                        rowIndex--; // Adjust rowIndex since the next row moves up after deletion
                        lastRow--; // Adjust rowCount after deletion
                    }
                }

                // Save changes
                workbook.SaveAs(csvFilePath);
                Console.WriteLine("Rows not containing the specified text in the specified column have been deleted.");
                CreateLogs("Rows not containing the specified text in the specified column have been deleted.", logFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();


                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (sheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }

        }


        public static void OptimizedFilterAndDelete(string filePath)
        {
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (!string.IsNullOrEmpty(newfilename))
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;

            // Load Excel application
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1]; // Assuming the first sheet
            Excel.Range usedRange = null;


            excelApp.DisplayAlerts = false;

            // Extract filter criteria
            string[] arr = args.Split('#');
            //  int columnIndex = int.Parse(arr[0]);
            string filterText = arr[0];
            string headerText = arr[1];
            //int startRowIndex = int.Parse(arr[3]);

            Excel.Range lastCell = worksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            usedRange = worksheet.UsedRange;

            int lastRow = usedRange.Rows.Count;


            try
            {
                workbook = excelApp.Workbooks.Open(filePath);
                worksheet = workbook.Sheets[1];
                usedRange = worksheet.UsedRange;

                // Find the column index by column name
                int columnIndex = -1;
                for (int col = 1; col <= usedRange.Columns.Count; col++)
                {
                    if (((Excel.Range)usedRange.Cells[1, col]).Value2.ToString() == headerText)
                    {
                        columnIndex = col;
                        break;
                    }
                }

                if (columnIndex == -1)
                {
                    throw new ArgumentException($"Column {headerText} not found.");
                }

                // Find the next available row for pasting (after the header)
                int pasteRow = 2; // Start pasting from the second row

                // Iterate through rows, filter and copy matching rows to a temporary list
                for (int row = 2; row <= lastRow; row++) // Start from 2 to skip header
                {
                    Excel.Range cell = (Excel.Range)usedRange.Cells[row, columnIndex];
                    string cellText = GetCellText(cell);
                    if (cellText != null && cellText.ToLower().Contains(filterText.ToLower()))
                    {
                        // Copy entire row to the new location
                        Excel.Range sourceRow = (Excel.Range)worksheet.Rows[row];
                        Excel.Range destinationRow = (Excel.Range)worksheet.Rows[pasteRow];
                        sourceRow.Copy(destinationRow);

                        // Increment pasteRow for the next row to paste
                        pasteRow++;
                    }
                }

                // Clear contents of rows below the last pasted row
                if (pasteRow <= lastRow)
                {
                    Excel.Range clearRange = worksheet.Rows[pasteRow].Resize[lastRow - pasteRow + 1];
                    clearRange.ClearContents();
                }
                // Save the new workbook
                workbook.SaveAs(csvFilePath);

            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();


                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }


            Console.WriteLine("Rows not containing the specified text in the specified column have been deleted.");
            CreateLogs("Rows not containing the specified text in the specified column have been deleted.", logFilePath);
        }

        private static string GetCellText(Excel.Range cell)
        {
            if (cell.Value2 != null)
            {
                if (cell.NumberFormat.ToString().Contains("d") || cell.NumberFormat.ToString().Contains("m") || cell.NumberFormat.ToString().Contains("y"))
                {
                    // Convert the serial number to a date string
                    DateTime dateValue = DateTime.FromOADate(cell.Value2);
                    return dateValue.ToShortDateString(); // or use dateValue.ToString("MM/dd/yyyy") for specific format
                }
                else
                {
                    return cell.Value2.ToString();
                }
            }
            return null;
        }

        public static void FilterAndDeleteRowsIfContains(string filePath)
        {
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(filePath);
            //outputFolder += "\\" + fileName;
            //string outputFolder = Path.Combine(Path.GetDirectoryName(filePath), "Output_Files");
            //string outputFile = Path.Combine(outputFolder, fileName);

            // Load Excel application
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[1]; // Assuming the first sheet
            excelApp.DisplayAlerts = false;

            // Extract filter criteria
            string[] arr = args.Split('#');
            int columnIndex = int.Parse(arr[0]);
            string searchText = arr[1];
            string headerText = arr[2];
            int startRowIndex = 1;
            if (arr.Length > 3)
            {
                startRowIndex = int.Parse(arr[3]);
            }
            // Filter and delete rows
            Excel.Range usedRange = sheet.UsedRange;
            int rowCount = usedRange.Rows.Count; // Assuming the first row is header
            try
            {
                for (int rowIndex = startRowIndex; rowIndex <= rowCount; rowIndex++)
                {
                    Excel.Range cell = (Excel.Range)sheet.Cells[rowIndex, columnIndex];
                    string cellContent = cell.Value != null ? cell.Value.ToString() : "";
                    if (!(cellContent.Contains(searchText) || cellContent.Equals(headerText)))
                    {
                        Excel.Range rowToDelete = (Excel.Range)sheet.Rows[rowIndex];
                        rowToDelete.Delete();
                        rowIndex--; // Adjust rowIndex since the next row moves up after deletion
                        rowCount--; // Adjust rowCount after deletion
                    }
                    else
                    {
                        int startRowIndex1 = 1;
                    }

                }

                // Save changes
                workbook.SaveAs(csvFilePath);
                Console.WriteLine("Rows not containing the specified text in the specified column have been deleted.");
                CreateLogs("Rows not containing the specified text in the specified column have been deleted.", logFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();


                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (sheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }


        }

        public static void FilterAndDeleteRowsIfFoundStartFromRow(string filePath)
        {
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(filePath);
            //outputFolder += "\\" + fileName;
            //string outputFolder = Path.Combine(Path.GetDirectoryName(filePath), "Output_Files");
            //string outputFile = Path.Combine(outputFolder, fileName);

            // Load Excel application
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[1]; // Assuming the first sheet

            excelApp.DisplayAlerts = false;
            // Extract filter criteria
            string[] arr = args.Split('#');
            int columnIndex = int.Parse(arr[0]);
            string searchText = arr[1];
            string headerText = arr[2];
            int startRowIndex = int.Parse(arr[3]);

            // Filter and delete rows
            Excel.Range usedRange = sheet.UsedRange;
            int rowCount = usedRange.Rows.Count;
            try
            {
                for (int rowIndex = startRowIndex; rowIndex <= rowCount; rowIndex++)
                {
                    Excel.Range cell = (Excel.Range)sheet.Cells[rowIndex, columnIndex];
                    string cellContent = cell.Value != null ? cell.Value.ToString() : "";
                    if (cellContent.Equals(searchText))
                    {
                        Excel.Range rowToDelete = (Excel.Range)sheet.Rows[rowIndex];
                        rowToDelete.Delete();
                        rowIndex--; // Adjust rowIndex since the next row moves up after deletion
                        rowCount--; // Adjust rowCount after deletion
                    }
                }

                // Save changes
                workbook.SaveAs(csvFilePath);
                Console.WriteLine("Rows not containing the specified text in the specified column have been deleted.");
                CreateLogs("Rows not containing the specified text in the specified column have been deleted.", logFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();


                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (sheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }


        }


        public static void FilterIfContainsStartFromRow(string filePath)
        {
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(filePath);
            //outputFolder += "\\" + fileName;
            //string outputFolder = Path.Combine(Path.GetDirectoryName(filePath), "Output_Files");
            //string outputFile = Path.Combine(outputFolder, fileName);

            // Load Excel application
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[1]; // Assuming the first sheet

            excelApp.DisplayAlerts = false;
            // Extract filter criteria
            string[] arr = args.Split('#');
            int columnIndex = int.Parse(arr[0]);
            string searchText = arr[1];
            string headerText = arr[2];
            int startRowIndex = int.Parse(arr[3]);

            // Filter and delete rows
            Excel.Range usedRange = sheet.UsedRange;
            int rowCount = usedRange.Rows.Count;
            // int startRowIndex = 1; // Assuming the first row is header
            try
            {
                for (int rowIndex = startRowIndex; rowIndex <= rowCount; rowIndex++)
                {
                    Excel.Range cell = (Excel.Range)sheet.Cells[rowIndex, columnIndex];
                    string cellContent = cell.Value != null ? cell.Value.ToString() : "";
                    if (!(cellContent.Contains(searchText) || cellContent.Equals(headerText)))
                    {
                        Excel.Range rowToDelete = (Excel.Range)sheet.Rows[rowIndex];
                        rowToDelete.Delete();
                        rowIndex--; // Adjust rowIndex since the next row moves up after deletion
                        rowCount--; // Adjust rowCount after deletion
                    }
                }

                // Save changes
                workbook.SaveAs(csvFilePath);
                Console.WriteLine("Rows not containing the specified text in the specified column have been deleted.");
                CreateLogs("Rows not containing the specified text in the specified column have been deleted.", logFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();


                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (sheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }


        }


        public static void DeleteRowsIfNotDouble(string filePath)
        {
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);

            if (!string.IsNullOrEmpty(newfilename))
            {
                fname = newfilename;
            }

            string csvFilePath = Path.Combine(outputFolder, fname + extension);

            string[] arr = args.Split('#');
            int columnIndex = int.Parse(arr[0]);
            int startRowIndex = int.Parse(arr[1]);

            // Load Excel application
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[1]; // Assuming the first sheet

            excelApp.DisplayAlerts = false;

            // Filter and delete rows
            Excel.Range usedRange = sheet.UsedRange;
            int rowCount = usedRange.Rows.Count;

            try
            {
                // Iterate through rows in reverse to avoid shifting issues
                for (int rowIndex = rowCount; rowIndex >= startRowIndex; rowIndex--)
                {
                    Excel.Range cell = (Excel.Range)sheet.Cells[rowIndex, columnIndex];
                    string cellContent = cell.Value != null ? cell.Value.ToString() : "";

                    // Check if the cell content is not a valid double
                    if (!double.TryParse(cellContent, out double result))
                    {
                        Excel.Range rowToDelete = (Excel.Range)sheet.Rows[rowIndex];
                        rowToDelete.Delete();
                    }
                }

                // Save changes
                workbook.SaveAs(csvFilePath);
                Console.WriteLine("Rows where the cell value is not a double have been deleted.");
                CreateLogs("Rows where the cell value is not a double have been deleted.", logFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();

                // Release COM objects
                System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }



        public static void PartialFilter(string filePath)
        {
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(filePath);
            //outputFolder += "\\" + fileName;
            //string outputFolder = Path.Combine(Path.GetDirectoryName(filePath), "Output_Files");
            //string outputFile = Path.Combine(outputFolder, fileName);

            // Load Excel application
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[1]; // Assuming the first sheet

            excelApp.DisplayAlerts = false;
            // Extract filter criteria
            string[] arr = args.Split('#');
            int columnIndex = int.Parse(arr[0]);
            string searchText = arr[1];
            string headerText = arr[2];

            // Filter and delete rows
            Excel.Range usedRange = sheet.UsedRange;
            int rowCount = usedRange.Rows.Count;
            int startRowIndex = 1; // Assuming the first row is header
            try
            {
                for (int rowIndex = startRowIndex; rowIndex <= rowCount; rowIndex++)
                {
                    Excel.Range cell = (Excel.Range)sheet.Cells[rowIndex, columnIndex];
                    string cellContent = cell.Value != null ? cell.Value.ToString() : "";
                    if (cellContent.Contains(searchText))
                    {
                        Excel.Range rowToDelete = (Excel.Range)sheet.Rows[rowIndex];
                        rowToDelete.Delete();
                        rowIndex--; // Adjust rowIndex since the next row moves up after deletion
                        rowCount--; // Adjust rowCount after deletion
                    }
                }

                // Save changes
                workbook.SaveAs(csvFilePath);
                Console.WriteLine("Rows not containing the specified text in the specified column have been deleted.");
                CreateLogs("Rows not containing the specified text in the specified column have been deleted.", logFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();


                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (sheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }


        }


        public static void DeleteIfPresent(string filePath)
        {
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(filePath);
            //outputFolder += "\\" + fileName;
            //string outputFolder = Path.Combine(Path.GetDirectoryName(filePath), "Output_Files");
            //string outputFile = Path.Combine(outputFolder, fileName);

            // Load Excel application
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[1]; // Assuming the first sheet

            excelApp.DisplayAlerts = false;
            // Extract filter criteria
            string[] arr = args.Split('#');
            int columnIndex = int.Parse(arr[0]);
            string searchText = arr[1];
            string headerText = arr[2];

            // Filter and delete rows
            Excel.Range usedRange = sheet.UsedRange;
            int rowCount = usedRange.Rows.Count;
            int startRowIndex = 1; // Assuming the first row is header
            try
            {
                for (int rowIndex = startRowIndex; rowIndex <= rowCount; rowIndex++)
                {
                    Excel.Range cell = (Excel.Range)sheet.Cells[rowIndex, columnIndex];
                    string cellContent = cell.Value != null ? cell.Value.ToString() : "";
                    if (cellContent.Equals(searchText))
                    {
                        Excel.Range rowToDelete = (Excel.Range)sheet.Rows[rowIndex];
                        rowToDelete.Delete();
                        rowIndex--; // Adjust rowIndex since the next row moves up after deletion
                        rowCount--; // Adjust rowCount after deletion
                    }
                }

                // Save changes
                workbook.SaveAs(csvFilePath);
                Console.WriteLine("Rows not containing the specified text in the specified column have been deleted.");
                CreateLogs("Rows not containing the specified text in the specified column have been deleted.", logFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();


                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (sheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }


        }

        //public static void FilterAndDeleteRows(string filePath)
        //{
        //    string fileName = Path.GetFileName(filePath);
        //    outputFolder += fileName;
        //    string[] arr = args.Split('#');

        //    int columnIndex = int.Parse(arr[0]);

        //    string searchText = arr[1];
        //    string headerText = arr[2];
        //    using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
        //    {
        //        IWorkbook workbook=null;

        //        // Identify the format (XLSX or XLS) and load the workbook accordingly
        //        if (Path.GetExtension(filePath).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
        //            workbook = new XSSFWorkbook(file);
        //        else if (Path.GetExtension(filePath).Equals(".xls", StringComparison.OrdinalIgnoreCase))
        //            workbook = new HSSFWorkbook(file);
        //       // HSSFWorkbook workbook = new HSSFWorkbook(file);
        //        // Get the first sheet from the workbook
        //        ISheet sheet = workbook.GetSheetAt(0);
        //        string sheetName = sheet.SheetName;
        //        // Convert the row enumerator to a list of IRow objects
        //        List<IRow> rows = new List<IRow>();
        //        var rowEnumerator = sheet.GetRowEnumerator();
        //        while (rowEnumerator.MoveNext())
        //        {
        //            rows.Add(rowEnumerator.Current as IRow);
        //        }
        //        int lastRowNum = sheet.LastRowNum; // Initial number of rows in the sheet


        //        // Filter rows based on the condition: if the cell in the specified column does not contain the search text

        //        int startRowIndex = 1;

        //        var rowsToDelete = rows
        //            .Skip(startRowIndex - 1) // Subtract 1 because row indexing is zero-based
        //            .Where(row =>
        //            {
        //                var cell = row.GetCell(columnIndex);
        //                var cellContent = cell == null ? string.Empty : cell.ToString();
        //                // Console.WriteLine($"Row: {row.RowNum}, Column: {columnIndex}, Content: {cellContent}");
        //                return !(cellContent.Equals(searchText) || cellContent.Equals(headerText));
        //            })
        //            .ToList();

        //        // Remove filtered rows from the sheet
        //        foreach (var row in rowsToDelete)
        //        {
        //            sheet.RemoveRow(row);
        //        }

        //        for (int rowIndex = sheet.LastRowNum; rowIndex >= 0; rowIndex--)
        //        {
        //            IRow row = sheet.GetRow(rowIndex);
        //            if (row == null || row.Cells.All(cell => cell == null || cell.CellType == CellType.Blank))
        //            {
        //                if (rowIndex >= 0)
        //                {
        //                    sheet.ShiftRows(rowIndex + 1, sheet.LastRowNum, -1); // Shift rows above
        //                }
        //            }
        //        }

        //        // Save the workbook to the target path
        //        using (FileStream outputStream = new FileStream(outputFolder, FileMode.Create, FileAccess.Write))
        //        {
        //            workbook.Write(outputStream);
        //        }


        //    }

        //    Console.WriteLine("Rows not containing the specified text in the specified column have been deleted.");
        //}
        static void IterateAllFilesInFolder(string folderPath, Action<string> Function, string outputFolder)
        {
            int filecount = 0;
            // Check if the directory exists
            if (Directory.Exists(folderPath))
            {
                // Get all files in the directory
                string[] files = Directory.GetFiles(folderPath);
                // Iterate over each file
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    string filepath = Path.GetFullPath(file);
                    if (changename.ToLower().Trim() == "true")
                    {
                        foreach (var kvp in nameconfig)
                        {
                            // Check if the keyword exists in the file name
                            if (fileName.ToLower().Contains(kvp.Key.ToLower()))
                            {
                                // If a match is found, call the associated function
                                newfilename = kvp.Value;
                            }
                        }

                    }

                    Function(file);
                    filecount++;
                }
            }
            else
            {
                Console.WriteLine("Directory does not exist.");
                CreateLogs("Directory does not exist.", logFilePath);
            }

            methodCount1++;
            Console.WriteLine("Total Files Converted " + filecount);
            CreateLogs("Total Files Converted " + filecount, logFilePath);

        }
        static void ClearFormatOfSheet(string filePath)
        {
            string logmessage = "";
            try
            {
                // Load the workbook (XLSX or XLS format)
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
                {
                    IWorkbook workbook;

                    // Identify the format (XLSX or XLS) and load the workbook accordingly
                    if (Path.GetExtension(filePath).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                        workbook = new XSSFWorkbook(fileStream);
                    else if (Path.GetExtension(filePath).Equals(".xls", StringComparison.OrdinalIgnoreCase))
                        workbook = new HSSFWorkbook(fileStream);
                    else
                    {
                        logmessage += Environment.NewLine + $"Unsupported file format: {filePath}";
                        CreateLogs(logmessage, logFilePath);

                        return;
                    }

                    // Iterate through all sheets
                    for (int i = 0; i < workbook.NumberOfSheets; i++)
                    {
                        ISheet sheet = workbook.GetSheetAt(i);

                        // Iterate through all rows and cells
                        foreach (IRow row in sheet)
                        {
                            foreach (ICell cell in row)
                            {
                                // Clear cell style
                                cell.CellStyle = null;
                            }
                        }
                    }

                    // Save the changes back to the file
                    using (var outputStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        workbook.Write(outputStream);
                    }
                    logmessage += Environment.NewLine + $"Cell styles cleared successfully in file: {filePath}";
                    CreateLogs(logmessage, logFilePath);
                }
            }
            catch (Exception ex)
            {
                logmessage += Environment.NewLine + $"Error clearing format of sheet in file '{filePath}': {ex.Message}";
                LogWithRed(logmessage);
                CreateLogs(logmessage, logFilePath);
            }

            Console.WriteLine(logmessage);
            CreateLogs(logmessage, logFilePath);

        }

        static void ClearFormat(string filePath)
        {
            string logmessage = "";
            Excel.Workbook workbook = null;
            Excel.Application excelApp = null;
            Excel.Worksheet worksheet = null;
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            try
            {

                excelApp = new Excel.Application();

                excelApp.DisplayAlerts = false; // Disable alerts to avoid any pop-ups during processing

                workbook = excelApp.Workbooks.Open(filePath);

                // Iterate through all sheets
                foreach (Excel.Worksheet sheet in workbook.Sheets)
                {
                    // Iterate through all rows and cells
                    foreach (Excel.Range row in sheet.UsedRange.Rows)
                    {
                        foreach (Excel.Range cell in row.Cells)
                        {
                            // Clear cell style
                            //cell.Style = null;
                            cell.ClearFormats();
                            //System.Runtime.InteropServices.Marshal.ReleaseComObject(cell);
                        }
                        // System.Runtime.InteropServices.Marshal.ReleaseComObject(row);

                    }

                    //System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                }

                // Save the changes back to the file


                // workbook.Save();
                workbook.SaveAs(csvFilePath);


                logmessage += Environment.NewLine + $"Cell styles cleared successfully in file: {filePath}";
                CreateLogs(logmessage, logFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                logmessage += Environment.NewLine + $"Error clearing format of sheet in file '{filePath}': {ex.Message}";
                LogWithRed(logmessage);
                CreateLogs(logmessage, logFilePath);
            }
            finally
            {
                methodCount++;
                // Close and release resources
                workbook.Close(false);
                excelApp.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }

            Console.WriteLine(logmessage);


        }

        static int[] ConvertToIntArray(string[] stringArray)
        {
            int[] intArray = new int[stringArray.Length];
            for (int i = 0; i < stringArray.Length; i++)
            {
                // Using int.Parse - Throws exception if parsing fails
                // intArray[i] = int.Parse(stringArray[i]);

                // Using int.TryParse - Returns true if parsing succeeds, false otherwise
                if (!int.TryParse(stringArray[i], out intArray[i]))
                {
                    // Handle parsing failure if needed
                    Console.WriteLine($"Failed to parse '{stringArray[i]}' as an integer.");
                    CreateLogs($"Failed to parse '{stringArray[i]}' as an integer.", logFilePath);
                    // You may choose to return null or handle the error differently
                }
            }
            return intArray;
        }

        static void ClearFormatParticularColumn(string filePath)
        {
            string[] arr = args.Split('#');
            int[] columnsToClear = ConvertToIntArray(arr);
            string logmessage = "";
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;

            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;


            try
            {
                excelApp = new Excel.Application();
                excelApp.DisplayAlerts = false; // Disable alerts to avoid any pop-ups during processing

                workbook = excelApp.Workbooks.Open(filePath);

                foreach (Excel.Worksheet sheet in workbook.Sheets)
                {
                    foreach (int columnNumber in columnsToClear)
                    {
                        Excel.Range column = sheet.Columns[columnNumber];
                        column.ClearFormats();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(column);
                    }
                }

                workbook.SaveAs(csvFilePath);
                workbook.Close(false);
                excelApp.Quit();
                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

                logmessage += Environment.NewLine + $"Cell styles cleared successfully for columns in file: {filePath}";
                CreateLogs(logmessage, logFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                logmessage += Environment.NewLine + $"Error clearing format of columns in file '{filePath}': {ex.Message}";
                LogWithRed(logmessage);
                CreateLogs(logmessage, logFilePath);
            }
            finally
            {
                methodCount++;
                if (workbook != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (excelApp != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }

            Console.WriteLine(logmessage);
            CreateLogs(logmessage, logFilePath);
        }

        static void ClearFormatworkbook(string filePath, Excel.Workbook workbook)
        {
            string logmessage = "";
            // Excel.Workbook workbook = null;

            try
            {


                // Iterate through all sheets
                foreach (Excel.Worksheet sheet in workbook.Sheets)
                {
                    // Iterate through all rows and cells
                    foreach (Excel.Range row in sheet.UsedRange.Rows)
                    {
                        foreach (Excel.Range cell in row.Cells)
                        {
                            // Clear cell style
                            //cell.Style = null;
                            cell.ClearFormats();
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(cell);
                        }
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(row);

                    }

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                }



                logmessage += Environment.NewLine + $"Cell styles cleared successfully in file: {filePath}";
                CreateLogs(logmessage, logFilePath);
            }
            catch (Exception ex)
            {
                logmessage += Environment.NewLine + $"Error clearing format of sheet in file '{filePath}': {ex.Message}";
                LogWithRed(logmessage);
                CreateLogs(logmessage, logFilePath);
            }
            finally
            {

            }

            Console.WriteLine(logmessage);
            CreateLogs(logmessage, logFilePath);

        }

        public static void SetColumnFormat(string filePath)
        {
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string[] arr = args.Split('#');
            List<string> columnNames = new List<string>(arr);
            string format = columnNames[0];
            int rowIndex = int.Parse(arr[1]);
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet worksheet = workbook.ActiveSheet;
            excelApp.DisplayAlerts = false;

            Excel.Range usedRange = worksheet.UsedRange;

            try
            {
                for (int i = 2; i < columnNames.Count; i++)
                {
                    Excel.Range targetColumn = null;


                    for (int columnIndex = 1; columnIndex <= usedRange.Columns.Count; columnIndex++)
                    {
                        Excel.Range cell = (Excel.Range)usedRange.Cells[rowIndex, columnIndex];
                        if (cell.Value2 != null && cell.Value2.ToString() == columnNames[i])
                        {
                            targetColumn = worksheet.Columns[columnIndex];
                            //targetColumn = worksheet.Range[cell, worksheet.Cells[worksheet.Rows.Count, cell.Column]];
                            break;
                        }
                        // Your logic here
                    }

                    if (targetColumn != null && format.Equals("Date"))
                    {
                        // Change the format of the target column to mm/dd/yyyy
                        targetColumn.NumberFormat = "MM/dd/yyyy";
                    }
                    else if (targetColumn != null && format.Equals("Number"))
                    {
                        // Read the entire column into a 2D array
                        object[,] cellValues = targetColumn.Value2 as object[,];

                        // Process the values in memory
                        if (cellValues != null)
                        {
                            for (int j = 1; j <= cellValues.GetLength(0); j++)
                            {
                                // Check if the cell has a value
                                if (cellValues[j, 1] != null && double.TryParse(cellValues[j, 1].ToString(), out double numberValue))
                                {
                                    // Convert the number to a string with 30 decimal places
                                    cellValues[j, 1] = numberValue.ToString("F30");
                                }
                            }

                            // Write the modified values back to Excel in one operation
                            targetColumn.Value2 = cellValues;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Column '{columnNames[i]}' not found.");
                    }
                }

                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();
            }
        }

        public static void FilterAndDeleteArgumentRowsList_Optimized(string filePath)
        {
            try
            {
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (!string.IsNullOrEmpty(newfilename))
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;
                // Ensure the file is a CSV
                if (Path.GetExtension(filePath).ToLower() != ".csv")
                {
                    throw new ArgumentException("The specified file is not a valid CSV file.");
                }

                // Extract filter criteria from args
                string[] arr;
                int columnIndex; // Default column index to use
                string searchText;
                string[] searchTextArr;
                List<string> searchTextList;
                int startRowIndex; // Keep it as is for logic handling
                string columnHeader; // 4th argument
                if (!string.IsNullOrEmpty(SplitParameter))
                {
                    char[] charArray = SplitParameter.ToCharArray();
                    string[] arr2 = args.Split(charArray[0]);
                    columnIndex = int.Parse(arr2[0]); // Default column index to use
                    searchText = arr2[1];
                    searchTextArr = searchText.Split(';');
                    searchTextList = searchTextArr.ToList();
                    startRowIndex = int.Parse(arr2[2]); // Keep it as is for logic handling
                    columnHeader = arr2.Length > 3 ? arr2[3] : ""; // 4th argument
                }
                else
                {
                    arr = args.Split('#');
                    columnIndex = int.Parse(arr[0]); // Default column index to use
                    searchText = arr[1];
                    searchTextArr = searchText.Split(';');
                    searchTextList = searchTextArr.ToList();
                    startRowIndex = int.Parse(arr[2]); // Keep it as is for logic handling
                    columnHeader = arr.Length > 3 ? arr[3] : ""; // 4th argument
                }
                // Read the CSV file content
                var lines = File.ReadAllLines(filePath);
                var remainingRecords = new List<string>();
                // Check for empty file
                if (lines.Length == 0)
                {
                    throw new InvalidOperationException("The CSV file is empty.");
                }

                // If columnHeader is provided, find its index
                if (!string.IsNullOrEmpty(columnHeader))
                {
                    // Assume the header is at startRowIndex
                    var headers = lines[startRowIndex].Split(',');

                    // Find the index of the specified column header
                    columnIndex = Array.FindIndex(headers, header => header.Trim().Equals(columnHeader, StringComparison.OrdinalIgnoreCase));
                    columnIndex++;
                    if (columnIndex == -1)
                    {
                        throw new ArgumentException($"Column header '{columnHeader}' not found.");
                    }

                    remainingRecords.Add(lines[startRowIndex]);
                    // Move startRowIndex to the next row, as the header row has been processed
                    startRowIndex++;
                }

                // Apply filtering based on the specified column and delete matching rows
                remainingRecords.AddRange(lines.Skip(startRowIndex)
                   .Where(line =>
                   {
                       // Find the position of (columnIndex - 1)th comma and the columnIndex comma
                       int previousCommaIndex = -1;  // Initial position before the start
                       int currentCommaIndex = -1;   // Tracks the position of the current comma

                       // Finding the previous comma (columnIndex - 1)
                       for (int i = 0; i < columnIndex; i++)
                       {
                           previousCommaIndex = currentCommaIndex;
                           currentCommaIndex = line.IndexOf(',', previousCommaIndex + 1);

                           // Handle the case where there are fewer commas than expected
                           if (currentCommaIndex == -1)
                           {
                               currentCommaIndex = line.Length; // If no more commas, take until the end
                               break;
                           }
                       }

                       // Extract the substring between previousCommaIndex and currentCommaIndex
                       string targetColumnValue;
                       if (currentCommaIndex != -1)
                       {
                           targetColumnValue = line.Substring(previousCommaIndex + 1, currentCommaIndex - previousCommaIndex - 1).Trim();
                       }
                       else
                       {
                           targetColumnValue = line.Substring(previousCommaIndex + 1).Trim();
                       }

                       // Apply the search text check on the extracted column
                       // Return TRUE for rows we want to delete, so negate the check to keep unmatched rows
                       return !searchTextList.Any(search =>
    (string.IsNullOrEmpty(search) ? string.IsNullOrEmpty(targetColumnValue) : targetColumnValue.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
);

                   })
                   .ToList());

                // Write the remaining records back to a new CSV file (after excluding matching rows)
                File.WriteAllLines(csvFilePath, remainingRecords.ToArray());

                Console.WriteLine("Non-matching records have been written to the output file, removing rows with the specified search text.");
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
                Console.WriteLine($"Error processing file '{filePath}': {ex.Message}");
                // Log the error as needed
            }

        }

        public static void FilterAndDeleteRowsListIfContainsStartFromRow_Optimized_Crestline(string filePath)
        {
            try
            {
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (!string.IsNullOrEmpty(newfilename))
                {
                    fname = newfilename;
                }
                if (!string.IsNullOrEmpty(changeextensionto))
                    extension = changeextensionto;

                string csvFilePath = outputFolder + @"\" + fname + extension;
                // Ensure the file is a CSV
                if (Path.GetExtension(filePath).ToLower() != ".csv")
                {
                    throw new ArgumentException("The specified file is not a valid CSV file.");
                }
                // Extract filter criteria from args
                string[] arr;
                int columnIndex; // Default column index to use
                string searchText;
                string[] searchTextArr;
                List<string> searchTextList;
                int startRowIndex; // Keep it as is for logic handling
                string columnHeader; // 4th argument
                if (!string.IsNullOrEmpty(SplitParameter))
                {
                    char[] charArray = SplitParameter.ToCharArray();
                    string[] arr2 = args.Split(charArray[0]);
                    columnIndex = int.Parse(arr2[0]); // Default column index to use
                    searchText = arr2[1];
                    searchTextArr = searchText.Split(';');
                    searchTextList = searchTextArr.ToList();
                    startRowIndex = int.Parse(arr2[2]); // Keep it as is for logic handling
                    columnHeader = arr2.Length > 3 ? arr2[3] : ""; // 4th argument
                }
                else
                {
                    arr = args.Split('#');
                    columnIndex = int.Parse(arr[0]); // Default column index to use
                    searchText = arr[1];
                    searchTextArr = searchText.Split(';');
                    searchTextList = searchTextArr.ToList();
                    startRowIndex = int.Parse(arr[2]); // Keep it as is for logic handling
                    columnHeader = arr.Length > 3 ? arr[3] : ""; // 4th argument
                }
                // Read the CSV file content
                var lines = File.ReadAllLines(filePath);
                var filteredRecords = new List<string>();
                // Check for empty file
                if (lines.Length == 0)
                {
                    throw new InvalidOperationException("The CSV file is empty.");
                }

                // If columnHeader is provided, find its index
                if (!string.IsNullOrEmpty(columnHeader))
                {
                    // Assume the header is at startRowIndex
                    //var headers = lines[startRowIndex].Split(',');
                    var headers = SplitCsvLine(lines[startRowIndex]);

                    // Find the index of the specified column header
                    columnIndex = Array.FindIndex(headers, header => header.Trim().Equals(columnHeader, StringComparison.OrdinalIgnoreCase));
                    //columnIndex++;
                    if (columnIndex == -1)
                    {
                        throw new ArgumentException($"Column header '{columnHeader}' not found.");
                    }

                    filteredRecords.Add(lines[startRowIndex]);
                    // Move startRowIndex to the next row, as the header row has been processed
                    startRowIndex++;
                }
                // Apply filtering based on the specified column and search text
                //               filteredRecords.AddRange(lines.Skip(startRowIndex)
                //                  .Where(line =>
                //                  {
                //                      int currentIndex = -1;
                //                      int previousIndex = -1;
                //                      Console.WriteLine("");
                //                      // Loop only until the desired columnIndex
                //                      for (int j = 0; j < columnIndex; j++)
                //                      {
                //                          previousIndex = currentIndex;
                //                          currentIndex = line.IndexOf(',', currentIndex + 1);

                //                          // If no more commas and we are at the target column, break
                //                          if (currentIndex == -1)
                //                          {
                //                              break;
                //                          }
                //                      }

                //                      // Extract the substring between commas
                //                      string targetColumnValue;
                //                      if (currentIndex != -1) // If target column exists
                //                      {
                //                          targetColumnValue = line.Substring(previousIndex + 1, currentIndex - previousIndex - 1).Trim();
                //                      }
                //                      else // If it's the last column or there's no comma at the target column
                //                      {
                //                          targetColumnValue = line.Substring(previousIndex + 1).Trim();
                //                      }

                //                      // Apply the search text check
                //                      //  return searchTextList.Any(search => targetColumnValue.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);
                //                      Console.WriteLine(targetColumnValue+"\n");
                //                      return searchTextList.All(search =>
                //    (string.IsNullOrEmpty(search) ? string.IsNullOrEmpty(targetColumnValue) : targetColumnValue.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                //);

                //                  })
                //                  .ToList());


                filteredRecords.AddRange(lines.Skip(startRowIndex)
           .Where(line =>
           {
               var columns = SplitCsvLine(line);
               if (columns.Length <= columnIndex)
               {
                   return false;
               }

               string targetColumnValue = columns[columnIndex].Trim();
               return searchTextList.All(search =>
                   string.IsNullOrEmpty(search) ? string.IsNullOrEmpty(targetColumnValue) : targetColumnValue.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);
           }).ToList());

                // Write the filtered records back to a new CSV file
                File.WriteAllLines(csvFilePath, filteredRecords.ToArray());

                Console.WriteLine("Filtered records have been written to the output file.");
            }
            catch (Exception ex)
            {
                LogWithRed(ex.Message.ToString());
                Console.WriteLine($"Error processing file '{filePath}': {ex.Message}");
                // Log the error as needed
            }
        }

        public static void FilterAndDeleteRowsListIfContainsStartFromRow_Optimized(string filePath)
        {
            try
            {
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (!string.IsNullOrEmpty(newfilename))
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;
                // Ensure the file is a CSV
                if (Path.GetExtension(filePath).ToLower() != ".csv")
                {
                    throw new ArgumentException("The specified file is not a valid CSV file.");
                }

                // Extract filter criteria from args
                string[] arr;
                int columnIndex; // Default column index to use
                string searchText;
                string[] searchTextArr;
                List<string> searchTextList;
                int startRowIndex; // Keep it as is for logic handling
                string columnHeader; // 4th argument
                if (!string.IsNullOrEmpty(SplitParameter))
                {
                    char[] charArray = SplitParameter.ToCharArray();
                    string[] arr2 = args.Split(charArray[0]);
                    columnIndex = int.Parse(arr2[0]); // Default column index to use
                    searchText = arr2[1];
                    searchTextArr = searchText.Split(';');
                    searchTextList = searchTextArr.ToList();
                    startRowIndex = int.Parse(arr2[2]); // Keep it as is for logic handling
                    columnHeader = arr2.Length > 3 ? arr2[3] : ""; // 4th argument
                }
                else
                {
                    arr = args.Split('#');
                    columnIndex = int.Parse(arr[0]); // Default column index to use
                    searchText = arr[1];
                    searchTextArr = searchText.Split(';');
                    searchTextList = searchTextArr.ToList();
                    startRowIndex = int.Parse(arr[2]); // Keep it as is for logic handling
                    columnHeader = arr.Length > 3 ? arr[3] : ""; // 4th argument
                }
                // Read the CSV file content
                var lines = File.ReadAllLines(filePath);
                var filteredRecords = new List<string>();
                // Check for empty file
                if (lines.Length == 0)
                {
                    throw new InvalidOperationException("The CSV file is empty.");
                }

                // If columnHeader is provided, find its index
                if (!string.IsNullOrEmpty(columnHeader))
                {
                    // Assume the header is at startRowIndex
                    var headers = lines[startRowIndex].Split(',');

                    // Find the index of the specified column header
                    columnIndex = Array.FindIndex(headers, header => header.Trim().Equals(columnHeader, StringComparison.OrdinalIgnoreCase));
                    columnIndex++;
                    if (columnIndex == -1)
                    {
                        throw new ArgumentException($"Column header '{columnHeader}' not found.");
                    }

                    filteredRecords.Add(lines[startRowIndex]);
                    // Move startRowIndex to the next row, as the header row has been processed
                    startRowIndex++;
                }
                // Apply filtering based on the specified column and search text
                filteredRecords.AddRange(lines.Skip(startRowIndex)
                   .Where(line =>
                   {
                       int currentIndex = -1;
                       int previousIndex = -1;

                       // Loop only until the desired columnIndex
                       for (int j = 0; j < columnIndex; j++)
                       {
                           previousIndex = currentIndex;
                           currentIndex = line.IndexOf(',', currentIndex + 1);

                           // If no more commas and we are at the target column, break
                           if (currentIndex == -1)
                           {
                               break;
                           }
                       }

                       // Extract the substring between commas
                       string targetColumnValue;
                       if (currentIndex != -1) // If target column exists
                       {
                           targetColumnValue = line.Substring(previousIndex + 1, currentIndex - previousIndex - 1).Trim();
                       }
                       else // If it's the last column or there's no comma at the target column
                       {
                           targetColumnValue = line.Substring(previousIndex + 1).Trim();
                       }

                       // Apply the search text check
                       //  return searchTextList.Any(search => targetColumnValue.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);

                       return searchTextList.Any(search =>
      (string.IsNullOrEmpty(search) ? string.IsNullOrEmpty(targetColumnValue) : targetColumnValue.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
  );

                   })
                   .ToList());

                // Write the filtered records back to a new CSV file
                File.WriteAllLines(csvFilePath, filteredRecords.ToArray());

                Console.WriteLine("Filtered records have been written to the output file.");
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
                Console.WriteLine($"Error processing file '{filePath}': {ex.Message}");
                // Log the error as needed
            }
        }


        public static void SortRowsByColumn(string filePath)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = null;
            excelApp.DisplayAlerts = false;
            Excel.Worksheet worksheet = null;

            string columnList = args.ToString();
            string[] columnListData = columnList.Split('#');

            string sortColumn = columnListData[0].ToString();
            string isAscending = columnListData[1].ToString();
            bool ascending = false;
            if (isAscending == "asc")
            {
                ascending = true;
            }

            try
            {
                workbook = excelApp.Workbooks.Open(filePath);
            }
            catch (Exception ex)
            {
                LogWithRed("Error opening Excel file: " + ex.Message);
                return;
            }

            try
            {
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;

                worksheet = (Excel.Worksheet)workbook.Sheets[1];

                Excel.Range usedRange = worksheet.UsedRange;
                Excel.Range sortRange = usedRange.Columns[sortColumn];

                Excel.Sort sort = worksheet.Sort;
                sort.SortFields.Clear();

                sort.SortFields.Add(
                    sortRange,
                    Excel.XlSortOn.xlSortOnValues,
                    ascending ? Excel.XlSortOrder.xlAscending : Excel.XlSortOrder.xlDescending,
                    Excel.XlSortDataOption.xlSortNormal
                );

                sort.SetRange(usedRange);
                sort.Header = Excel.XlYesNoGuess.xlYes;
                sort.MatchCase = false;
                sort.Orientation = Excel.XlSortOrientation.xlSortColumns;
                sort.SortMethod = Excel.XlSortMethod.xlPinYin;

                sort.Apply();

                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
            }
            finally
            {
                methodCount++;
                workbook.Close(false);
                excelApp.Quit();

                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        public static void FindCellValueAndReplaceIfContains(string filePath)
        {
            Application excelApp = null;
            Workbook workbook = null;
            Worksheet worksheet = null;
            try
            {


                string[] arr = args.Split('#');
                int columnIndex = int.Parse(arr[0]);
                string targetValue = arr[1];
                string replaceValue = arr[2];
                // Create an instance of Excel Application
                excelApp = new Application();
                excelApp.DisplayAlerts = false;

                // Open the Excel workbook
                workbook = excelApp.Workbooks.Open(filePath);

                // Get the worksheet
                worksheet = (Worksheet)workbook.Sheets[1];

                // Get the used range of the worksheet
                Range usedRange = worksheet.UsedRange;

                // Get the values of the entire column
                Range columnRange = usedRange.Columns[columnIndex];

                // Get the cell values as an array
                object[,] values = (object[,])columnRange.Value;
                // Loopobject[,] values =  through each cell in the column
                for (int i = 2; i <= values.GetLength(0); i++)
                {
                    // Check if the cell value matches the target value
                    if (values[i, 1] != null && values[i, 1].ToString().Contains(targetValue))
                    {
                        string cellValue = values[i, 1].ToString();
                        // Replace all occurrences of "M" with "000"
                        values[i, 1] = cellValue.Replace(targetValue, replaceValue);
                    }
                }

                // Update the column range with the new values
                columnRange.Value = values;

                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);

                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;
                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
            finally
            {
                methodCount++;
                workbook.Close();

                // Quit Excel application
                excelApp.Quit();

                // Clean up
                ReleaseObject(worksheet);
                ReleaseObject(workbook);
                ReleaseObject(excelApp);
            }
        }

        public static void MergeMultipleFiles(string filePath)
        {
            Excel.Workbook workbook = null;
            Excel.Application excelApp = null;
            Excel.Workbook mergedWorkbook = null;
            string csvFilePath = "";
            try
            {
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (!string.IsNullOrEmpty(newfilename))
                {
                    fname = newfilename;
                }

                csvFilePath = outputFolder + @"\" + fname + extension;
                string[] arr = args.Split('#');
                string filePattern = arr[0];
                int headerRow = int.Parse(arr[1]);
                int startRow = int.Parse(arr[2]);
                int currentRow = 1;

                // Initialize Excel application
                excelApp = new Excel.Application();
                mergedWorkbook = excelApp.Workbooks.Add();
                Excel.Worksheet mergedSheet = mergedWorkbook.Sheets[1];

                bool isHeaderCopied = false;

                // Loop through all files matching the pattern
                foreach (string path in Directory.GetFiles(inputFolder, filePattern))
                {
                    // Open the Excel file
                    workbook = excelApp.Workbooks.Open(path);
                    Excel.Worksheet worksheet = workbook.Sheets[1];
                    Excel.Range usedRange = worksheet.UsedRange;

                    object[,] fileData = usedRange.Value2;
                    int rows = usedRange.Rows.Count;
                    int columns = usedRange.Columns.Count;

                    // Copy header row from the first file only
                    if (!isHeaderCopied)
                    {
                        for (int i = 1; i <= headerRow; i++)
                        {
                            var headerRange = mergedSheet.Range[mergedSheet.Cells[currentRow, 1], mergedSheet.Cells[currentRow, columns]];
                            var headerData = new object[1, columns];

                            for (int col = 1; col <= columns; col++)
                            {
                                headerData[0, col - 1] = fileData[i, col]; // Copy header row
                            }

                            headerRange.Value2 = headerData;
                            currentRow++;
                        }
                        isHeaderCopied = true; // Mark that header is copied
                    }

                    // Copy the data from the current file, starting after the header row
                    for (int row = startRow; row <= rows; row++)
                    {
                        if (!IsRowEmpty(fileData, row, columns))  // Check if the row is empty
                        {
                            var dataRange = mergedSheet.Range[mergedSheet.Cells[currentRow, 1],
                                mergedSheet.Cells[currentRow, columns]];

                            var dataToWrite = new object[1, columns];  // Write one row at a time
                            for (int col = 1; col <= columns; col++)
                            {
                                dataToWrite[0, col - 1] = fileData[row, col];
                            }

                            dataRange.Value2 = dataToWrite;  // Write the row data
                                                             // Copy format from source worksheet to merged sheet
                            Excel.Range sourceRange = worksheet.Range[worksheet.Cells[row, 1], worksheet.Cells[row, columns]];
                            Excel.Range targetRange = mergedSheet.Range[mergedSheet.Cells[currentRow, 1], mergedSheet.Cells[currentRow, columns]];
                            sourceRange.Copy();
                            targetRange.PasteSpecial(Excel.XlPasteType.xlPasteFormats);
                            currentRow++;  // Move the current row forward
                        }
                    }
                }
                workbook.Close(false);


                // Save the merged workbook after processing all files
                mergedWorkbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
            finally
            {
                // Close the current workbook but don't quit Excel yet
                methodCount++;// Save the merged workbook after processing all files
                mergedWorkbook.SaveAs(csvFilePath);
                mergedWorkbook.Close();
                excelApp.Quit();
            }


        }

        static bool IsRowEmpty(object[,] data, int row, int columns)
        {
            for (int col = 1; col <= columns; col++)
            {
                var cellValue = data[row, col];  // Retrieve the cell value once

                // Check if the cell is neither null nor an empty string
                if (cellValue != null && !(cellValue is string str && str.Length == 0))
                {
                    return false;  // If any cell in the row has data, it's not empty
                }
            }
            return true;  // The row is empty if all cells are null or empty
        }

        public static string ConvertRangeToString(object[,] range, int rows, int columns)
        {
            StringBuilder result = new StringBuilder();  // Use StringBuilder for better performance

            for (int row = 1; row <= rows; row++)
            {
                for (int col = 1; col <= columns; col++)
                {
                    result.Append(range[row, col]?.ToString() ?? "");  // Handle null values safely
                    result.Append(",");  // Use a delimiter between cells
                }
                result.AppendLine();  // Use newline between rows
            }

            return result.ToString();
        }

        public static void RenameFilesCondition(string filePath1)
        {
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            try
            {
                string extension = Path.GetExtension(filePath1);
                string csvFilePath = string.Empty;
                string[] arr = args.Split('#');
                string filePattern = arr[0];
                string targetColumn = arr[1];
                string searchText = arr[2];
                string[] searchTextArr = searchText.Split('@');

                int startRowIndex = int.Parse(arr[3]); // Keep it as is for logic handling
                int columnHeader = int.Parse(arr[4]); // 4th argument
                string folderPath = inputFolder;
                string[] excelFiles = Directory.GetFiles(folderPath, filePattern);

                excelApp = new Excel.Application();
                excelApp.Visible = false;

                Dictionary<string, string> searchTextDict = new Dictionary<string, string>();
                foreach (string item in searchTextArr)
                {
                    string[] parts = item.Split(':');
                    if (parts.Length == 2)
                    {
                        searchTextDict[parts[1]] = parts[0];
                    }
                }

                foreach (string filePath in excelFiles)
                {
                    bool sheetRenamed = false;
                    string fname = Path.GetFileNameWithoutExtension(filePath);
                    workbook = excelApp.Workbooks.Open(filePath);
                    Excel._Worksheet worksheet = workbook.Sheets[1]; // Assuming data is in the first sheet
                    Excel.Range fundColumn = worksheet.Columns[targetColumn]; // Change this if Fund column is not A

                    // Check the values in the Fund column
                    foreach (var key in searchTextDict.Keys)
                    {
                        foreach (string x in searchTextDict[key].Split(';'))
                        {
                            Excel.Range foundCell = fundColumn.Find(x, Type.Missing, Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart);

                            if (foundCell != null)
                            {
                                // Construct the file path based on the found value
                                csvFilePath = Path.Combine(outputFolder, fname + key + extension);
                                sheetRenamed = true;
                                break;
                            }
                        }
                        if (sheetRenamed)
                            break;
                    }

                    if (csvFilePath != filePath)
                    {
                        workbook.SaveAs(csvFilePath);
                        workbook.Close(false);
                        //File.Move(filePath, csvFilePath);
                        Console.WriteLine($"File renamed to {csvFilePath}");
                    }
                    else
                    {
                        workbook.Close(false);
                        Console.WriteLine($"No changes made for {filePath}");
                    }
                }
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
            finally
            {
                methodCount++;
                workbook.Close(false);
                excelApp.Quit();
            }
        }

        public static void PerformSubstraction(string filePath)
        {
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            try
            {
                int filterColumnNumber;
                string targetValue;
                int sourceColumnNumber1;
                int sourceColumnNumber2;

                int targetColumnNumber;
                int startFilterRow;
                string[] arr = args.Split('#');

                int startRowIndex = int.Parse(arr[3]);

                sourceColumnNumber1 = int.Parse(arr[0]);
                sourceColumnNumber2 = int.Parse(arr[1]);
                targetColumnNumber = int.Parse(arr[2]);


                ///////////////////////////////////////////////////////////////////
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;
                string fileName = Path.GetFileName(filePath);
                //outputFolder += "\\" + fileName;
                //string outputFolder = Path.Combine(Path.GetDirectoryName(filePath), "Output_Files");
                //string outputFile = Path.Combine(outputFolder, fileName);

                // Load Excel application
                excelApp = new Excel.Application();
                workbook = excelApp.Workbooks.Open(filePath);
                Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[1]; // Assuming the first sheet

                excelApp.DisplayAlerts = false;
                // Extract filter criteria

                // Filter and delete rows
                Excel.Range usedRange = sheet.UsedRange;
                int rowCount = usedRange.Rows.Count;


                for (int rowIndex = startRowIndex; rowIndex <= rowCount; rowIndex++)
                {


                    Excel.Range cell1 = (Excel.Range)sheet.Cells[rowIndex, sourceColumnNumber1];
                    Excel.Range cell2 = (Excel.Range)sheet.Cells[rowIndex, sourceColumnNumber2];
                    Excel.Range resultCell = (Excel.Range)sheet.Cells[rowIndex, targetColumnNumber];
                    double value1;
                    double value2;
                    Console.WriteLine(cell1.Value);
                    if (cell1.Value != null && cell2.Value != null)
                    {
                        value1 = Convert.ToDouble(cell1.Value);
                        value2 = Convert.ToDouble(cell2.Value);
                        double result = value1 - value2;
                        resultCell.Value = result;
                    }
                    else
                    {
                        resultCell.Value = "";
                    }

                }


                // Save changes
                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();
            }
            Console.WriteLine("Rows not containing the specified text in the specified column have been deleted.");
        }

        public static void Deleterows(string filePath)
        {
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null; // Assuming first sheet
            try
            {
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (!string.IsNullOrEmpty(newfilename))
                {
                    fname = newfilename;
                }
                if (!string.IsNullOrEmpty(changeextensionto))
                    extension = changeextensionto;

                string csvFilePath = outputFolder + @"\" + fname + extension;
                // Ensure the file is a CSV
                if (Path.GetExtension(filePath).ToLower() != ".csv")
                {
                    throw new ArgumentException("The specified file is not a valid CSV file.");
                }
                // Extract filter criteria from args
                string[] arr = args.Split('#'); ;
                int targetColumn = int.Parse(arr[0]); // Default column index to use
                string deletionValue = arr[1];
                //string[] searchTextArr = searchText.Split(';'); ;
                //List<string> searchTextList;
                int startRowIndex = int.Parse(arr[3]) + 1; // Keep it as is for logic handling
                string columnHeader = arr[2]; // 4th argument
                                              // Start Excel application
                excelApp = new Excel.Application();
                workbook = excelApp.Workbooks.Open(filePath);
                worksheet = workbook.Sheets[1]; // Assuming first sheet

                // Get the used range of the worksheet
                Excel.Range usedRange = worksheet.UsedRange;

                // Apply AutoFilter to filter rows where the target column contains deletionValue
                Excel.Range columnRange = worksheet.Columns[targetColumn];
                columnRange.AutoFilter(
                Field: 1,
                Criteria1: $"={deletionValue}*", // Filter based on partial match
                Operator: Excel.XlAutoFilterOperator.xlAnd
            );

                // Get the rows that are visible (filtered rows)
                Excel.Range visibleRows = usedRange.SpecialCells(Excel.XlCellType.xlCellTypeVisible);

                // Delete the visible rows
                visibleRows.EntireRow.Delete(Excel.XlDeleteShiftDirection.xlShiftUp);

                // Remove the filter
                worksheet.AutoFilterMode = false;

                // Save and close workbook
                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
            finally
            {
                methodCount++;
                workbook.Close(false);
                excelApp.Quit();

                // Release COM objects
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        private static string[] SplitCsvLine(string line)
        {
            // This regular expression splits the CSV by commas, but skips commas inside quotes
            string pattern = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
            return Regex.Split(line, pattern).Select(s => s.Trim('"')).ToArray();
        }

        static void UpdateCYMIFile(string FilePath)
        {
            Application excelApp = null;
            Workbook srcWorkbook = null;


            // Open the destination workbook and worksheet
            Workbook destWorkbook = null;



            try
            {
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string fname = Path.GetFileNameWithoutExtension(FilePath);
                string extension = Path.GetExtension(FilePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string textFile = outputFolder + @"\ExtraTrades.txt";
                string csvFilePath = outputFolder + @"\" + fname + extension;
                excelApp = new Application();
                excelApp.DisplayAlerts = false;
                var columnMappings = new Dictionary<string, int>();
                var srcColumnMappings = new Dictionary<string, int>();
                // Extract filter criteria
                string[] arr = args.Split('#');
                string destFileName = FilePath;
                string srcFileName = currentDirectory + "\\" + arr[0];
                int headerRowIndex = int.Parse(arr[1]);
                int destColumnIndex = int.Parse(arr[2]);
                // Open the source workbook and worksheet
                srcWorkbook = excelApp.Workbooks.Open(srcFileName);
                Worksheet srcWorksheet = srcWorkbook.Sheets[1];

                // Open the destination workbook and worksheet
                destWorkbook = excelApp.Workbooks.Open(destFileName);
                Worksheet destWorksheet = destWorkbook.Sheets[1];

                // Read source data into a dictionary
                Dictionary<string, Dictionary<string, string>> srcDict = new Dictionary<string, Dictionary<string, string>>();
                int srcLastRow = srcWorksheet.Cells[srcWorksheet.Rows.Count, 1].End(XlDirection.xlUp).Row;

                //Read destination's column headers
                Excel.Range headerRow = destWorksheet.Rows[headerRowIndex];
                for (int col = 1; col <= destColumnIndex; col++)
                {
                    Excel.Range cell = headerRow.Cells[1, col] as Excel.Range;
                    if (cell != null && cell.Value2 != null)
                    {
                        string columnName = cell.Value2.ToString();
                        columnMappings[columnName] = col;
                    }
                }

                //Read source's column headers
                Excel.Range headerRow1 = srcWorksheet.Rows[1];
                for (int col = 1; col <= 10; col++)
                {
                    Excel.Range cell = headerRow1.Cells[1, col] as Excel.Range;
                    if (cell != null && cell.Value2 != null)
                    {
                        string columnName = cell.Value2.ToString();
                        srcColumnMappings[columnName] = col;
                    }
                }

                for (int i = 2; i <= srcLastRow; i++) // Assuming the first row is headers
                {
                    string symbol = string.Empty;
                    Excel.Range cell = srcWorksheet.Cells[i, srcColumnMappings["OSISymbol"]] as Excel.Range;
                    if (cell?.Value2 != null)
                        symbol = srcWorksheet.Cells[i, srcColumnMappings["OSISymbol"]].Value.ToString();
                    else
                        symbol = srcWorksheet.Cells[i, srcColumnMappings["Symbol"]].Value.ToString();
                    string account = srcWorksheet.Cells[i, srcColumnMappings["ACCOUNT"]].Value.ToString();
                    string strategy = srcWorksheet.Cells[i, srcColumnMappings["Strategy"]].Value.ToString();

                    if (!srcDict.ContainsKey(symbol))
                    {
                        Dictionary<string, string> tempDict = new Dictionary<string, string>();
                        tempDict[account] = strategy;
                        srcDict[symbol] = tempDict;
                    }
                    else if (srcDict.ContainsKey(symbol))
                    {
                        if (!srcDict[symbol].ContainsKey(account))
                        {
                            srcDict[symbol].Add(account, strategy);
                        }
                        else if (srcDict[symbol][account] != strategy)
                        {
                            Console.WriteLine("\n We have multiple strategy for account:" + account);
                        }
                    }
                }

                // Update destination data
                int destLastRow = destWorksheet.Cells[destWorksheet.Rows.Count, 1].End(XlDirection.xlUp).Row;

                using (StreamWriter writer = new StreamWriter(textFile, append: true))
                {
                    writer.WriteLine("Not found following symbols:");
                    for (int i = 3; i < destLastRow; i++) // Assuming the first row is headers
                    {
                        string symbol = destWorksheet.Cells[i, columnMappings["Symbol"]].Value.ToString();
                        string account = destWorksheet.Cells[i, columnMappings["Account"]].Value.ToString();
                        if (srcDict.ContainsKey(symbol))
                        {
                            bool accountFound = false;
                            foreach (var scc in srcDict[symbol].Keys)
                            {
                                if (scc.Contains(account))
                                {
                                    destWorksheet.Cells[i, destColumnIndex].Value = srcDict[symbol][scc];
                                    accountFound = true;
                                    break;
                                }
                            }
                            if (!accountFound)
                            {
                                Console.WriteLine("Not found " + account + " for " + symbol + " in exported data.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Not found " + symbol + " in exported data.");
                            writer.WriteLine("\n" + symbol);
                        }
                    }
                }

                // Save and close the destination workbook
                destWorkbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
            finally
            {
                methodCount++;
                destWorkbook.Close(false);

                // Close the source workbook without saving changes
                srcWorkbook.Close(false);

                // Quit the Excel application
                excelApp.Quit();
            }
        }

        static void ConvertexttoColInMemory(string excelFilePath)
        {
            string csvFilePath = "";
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;
            string[] arr = args.Split('#');
            string delimiter = arr[0];
            try
            {
                string fname = newfilename != "" ? newfilename : Path.GetFileNameWithoutExtension(excelFilePath);
                csvFilePath = Path.Combine(outputFolder, $"{fname}.csv");

                excelApp = new Excel.Application { DisplayAlerts = false };
                workbook = excelApp.Workbooks.Open(excelFilePath);
                worksheet = (Excel.Worksheet)workbook.Sheets[1];

                // Read data into memory (2D array)
                Excel.Range usedRange = worksheet.UsedRange;
                object[,] dataArray = (object[,])usedRange.Value2;

                // Split the data manually using delimiter
                List<string[]> processedData = new List<string[]>();

                for (int i = 1; i <= dataArray.GetLength(0); i++)
                {
                    string rowString = dataArray[i, 1]?.ToString() ?? ""; // Get the row data as a string
                    string[] columns = rowString.Split(new string[] { delimiter }, StringSplitOptions.None); // Split by delimiter
                    processedData.Add(columns);
                }

                // Write to CSV file
                using (StreamWriter sw = new StreamWriter(csvFilePath))
                {
                    foreach (var row in processedData)
                    {
                        sw.WriteLine(string.Join(",", row));
                    }
                }

                Console.WriteLine($"Conversion text to column completed for {fname}");
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed($"Error: {ex.Message}");
            }
            finally
            {
                methodCount++;
                workbook?.Close(false);
                excelApp?.Quit();
                ReleaseObject(worksheet);
                ReleaseObject(workbook);
                ReleaseObject(excelApp);
            }
        }
       
        public static void AppendDataToFile_Optimized(string mainFilePath)
        {
            try
            {
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string fname = Path.GetFileNameWithoutExtension(mainFilePath);
                string extension = Path.GetExtension(mainFilePath);
                if (!string.IsNullOrEmpty(newfilename))
                {
                    fname = newfilename;
                }

                string csvFilePath = Path.Combine(outputFolder, fname + extension);
                string partialFilePath = currentDirectory;

                // Split the arguments (assumed format is partialFilePath#partialFileName#startRow)
                string[] arr = args.Split('#');
                if (!arr[0].Equals("NA"))
                {
                    partialFilePath = Path.Combine(currentDirectory, arr[0]);
                }

                string partialFileName = arr[1];
                int startRow = int.Parse(arr[2]);

                // Find matching files in the directory
                string[] matchingFiles = FindFilesWithPartialName(partialFilePath, partialFileName);
                if (matchingFiles.Length == 0)
                {
                    Console.WriteLine("No matching files found.");
                    return;
                }

                string otherFilePath = matchingFiles[0];

                // Ensure both files exist
                if (!File.Exists(mainFilePath))
                {
                    Console.WriteLine("Main file does not exist.");
                    return;
                }

                if (!File.Exists(otherFilePath))
                {
                    Console.WriteLine("Other file does not exist.");
                    return;
                }

                // Read all lines from both files
                List<string> mainFileLines = File.ReadAllLines(mainFilePath).ToList();
                List<string> otherFileLines = File.ReadAllLines(otherFilePath).Skip(startRow - 1).ToList(); // Skip rows till startRow in the second file

                // Append the other file's lines to the main file
                mainFileLines.AddRange(otherFileLines);

                // Write back the combined data to the main file (or a new file, if specified)
                File.WriteAllLines(csvFilePath, mainFileLines);

                Console.WriteLine("Data appended successfully to: " + csvFilePath);
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("An error occurred: " + ex.Message);
            }
        }

        public static void FilterAndReplaceValueInColumnList_Optimized(string filePath)
        {
            try
            {
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (!string.IsNullOrEmpty(newfilename))
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;
                // Ensure the file is a CSV
                if (Path.GetExtension(filePath).ToLower() != ".csv")
                {
                    throw new ArgumentException("The specified file is not a valid CSV file.");
                }

                // Process arguments and extract filter criteria
                string[] arr;
                int columnIndex; // Column index to search in
                string searchText; // Text to search for
                string[] searchTextArr; // Array of search substrings
                List<string> searchTextList; // List of search substrings
                int startRowIndex; // Starting row index for processing
                string columnHeader; // Column header name
                string replacementText; // Text to replace the search substring with
                string[] replacementArr; // Array of replacement values
                List<string> replacementList; // List of replacement values

                // Check if SplitParameter is provided for custom splitting logic
                if (!string.IsNullOrEmpty(SplitParameter))
                {
                    char[] charArray = SplitParameter.ToCharArray();
                    string[] arr2 = args.Split(charArray[0]);

                    columnIndex = int.Parse(arr2[0]);
                    searchText = arr2[1];
                    searchTextArr = searchText.Split(';');
                    searchTextList = searchTextArr.ToList();

                    startRowIndex = int.Parse(arr2[2]);
                    replacementText = arr2[3];
                    replacementArr = replacementText.Split(';');
                    replacementList = replacementArr.ToList();

                    columnHeader = arr2.Length > 4 ? arr2[4] : "";
                }
                else
                {
                    arr = args.Split('#');
                    columnIndex = int.Parse(arr[0]);
                    searchText = arr[1];
                    searchTextArr = searchText.Split(';');
                    searchTextList = searchTextArr.ToList();

                    startRowIndex = int.Parse(arr[2]);
                    replacementText = arr[3];
                    replacementArr = replacementText.Split(';');
                    replacementList = replacementArr.ToList();

                    columnHeader = arr.Length > 4 ? arr[4] : "";
                }
                // Read the CSV file content
                var lines = File.ReadAllLines(filePath);
                var filteredRecords = new List<string>();
                // Check for empty file
                if (lines.Length == 0)
                {
                    throw new InvalidOperationException("The CSV file is empty.");
                }

                // If columnHeader is provided, find its index
                if (!string.IsNullOrEmpty(columnHeader))
                {
                    // Assume the header is at startRowIndex
                    var headers = lines[startRowIndex].Split(',');

                    // Find the index of the specified column header
                    columnIndex = Array.FindIndex(headers, header => header.Trim().Equals(columnHeader, StringComparison.OrdinalIgnoreCase));
                    columnIndex++;
                    if (columnIndex == -1)
                    {
                        throw new ArgumentException($"Column header '{columnHeader}' not found.");
                    }

                    filteredRecords.Add(lines[startRowIndex]);
                    // Move startRowIndex to the next row, as the header row has been processed
                    startRowIndex++;
                }
                // Apply filtering based on the specified column and search text
                filteredRecords.AddRange(lines.Skip(startRowIndex)
     .Select(line =>
     {
         int currentIndex = -1;
         int previousIndex = -1;

         // Loop only until the desired columnIndex
         for (int j = 0; j < columnIndex; j++)
         {
             previousIndex = currentIndex;
             currentIndex = line.IndexOf(',', currentIndex + 1);

             // If no more commas and we are at the target column, break
             if (currentIndex == -1)
             {
                 break;
             }
         }

         // Extract the substring between commas
         string targetColumnValue;
         if (currentIndex != -1) // If target column exists
         {
             targetColumnValue = line.Substring(previousIndex + 1, currentIndex - previousIndex - 1).Trim();
         }
         else // If it's the last column or there's no comma at the target column
         {
             targetColumnValue = line.Substring(previousIndex + 1).Trim();
         }

         // Check if the target column contains any of the search substrings
         for (int i = 0; i < searchTextList.Count; i++)
         {
             if (targetColumnValue.IndexOf(searchTextList[i]) >= 0)
             {
                 // Replace the search substring with the corresponding replacement value
                 targetColumnValue = targetColumnValue.Replace(searchTextList[i], replacementList[i]);
             }
         }

         // Reconstruct the line with the updated target column value
         StringBuilder newLine = new StringBuilder();

         // Re-add everything before the target column
         if (previousIndex != -1)
         {
             newLine.Append(line.Substring(0, previousIndex + 1));
         }

         // Append the modified target column value
         newLine.Append(targetColumnValue);

         // Re-add everything after the target column
         if (currentIndex != -1)
         {
             newLine.Append(line.Substring(currentIndex));
         }

         // Return the reconstructed line
         return newLine.ToString();
     })
     .ToList());

                // Write the filtered records back to a new CSV file
                File.WriteAllLines(csvFilePath, filteredRecords.ToArray());

                Console.WriteLine("Filtered records have been written to the output file.");
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed($"Error processing file '{filePath}': {ex.Message}");
                // Log the error as needed
            }
        }

        static void ReplaceCommasWithBlanks(string filePath)
        {
            string csvFilePath = "";
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;
            string[] arr = args.Split('#');
            string delimiter = arr[0];
            try
            {
                string fname = newfilename != "" ? newfilename : Path.GetFileNameWithoutExtension(filePath);
                csvFilePath = Path.Combine(outputFolder, $"{fname}.csv");


                string fileContent = File.ReadAllText(filePath);

                // Use StringBuilder for better performance when replacing large amounts of data
                StringBuilder modifiedContent = new StringBuilder(fileContent.Length);

                // Loop through the file content and replace commas with blanks
                foreach (char c in fileContent)
                {
                    if (c == ',')
                    {
                        // Replace comma with a blank (empty string)
                        modifiedContent.Append(""); // No need to add anything for a blank
                    }
                    else
                    {
                        // Add the original character to the StringBuilder
                        modifiedContent.Append(c);
                    }
                }

                // Write the modified content back to the file (or create a new file)
                File.WriteAllText(csvFilePath, modifiedContent.ToString());

                Console.WriteLine($"All commas replaced with blanks in file: {csvFilePath}");
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed($"Error while replacing commas with blanks: {ex.Message}");
            }
        }

        public static void ModifySymbolAndULSymbolBasedOnDescription(string filePath)
        {
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;
                string fileName = Path.GetFileName(filePath);
                //outputFolder += "\\" + fileName;
                //string outputFolder = Path.Combine(Path.GetDirectoryName(filePath), "Output_Files");
                //string outputFile = Path.Combine(outputFolder, fileName);

                // Load Excel application
                excelApp = new Excel.Application();
                workbook = excelApp.Workbooks.Open(filePath);
                worksheet = (Excel.Worksheet)workbook.Sheets[1]; // Assuming the first sheet

                excelApp.DisplayAlerts = false;
                // Extract filter criteria
                string[] arr = args.Split('#');
                int headerRow = int.Parse(arr[0]);

                // Find header row and columns
                Excel.Range usedRange = worksheet.UsedRange;
                int descriptionCol = 0;
                int symbolCol = 0;
                int ulSymbolCol = 0;

                // Locate the header row columns
                for (int i = 1; i <= usedRange.Columns.Count; i++)
                {
                    var headerValue = (usedRange.Cells[headerRow, i] as Excel.Range).Value2?.ToString();
                    if (headerValue == "Description") descriptionCol = i;
                    if (headerValue == "Symbol") symbolCol = i;
                    if (headerValue == "ULSymbol") ulSymbolCol = i;
                }

                if (descriptionCol == 0 || symbolCol == 0 || ulSymbolCol == 0)
                {
                    Console.WriteLine("Description, Symbol, or ULSymbol column not found.");
                    return;
                }

                // Iterate through rows and modify as needed
                for (int i = headerRow + 1; i <= usedRange.Rows.Count; i++) // Start from the row after headers
                {
                    var descriptionValue = (usedRange.Cells[i, descriptionCol] as Excel.Range).Value2?.ToString();
                    var symbolValue = (usedRange.Cells[i, symbolCol] as Excel.Range).Value2?.ToString();
                    var ulSymbolValue = (usedRange.Cells[i, ulSymbolCol] as Excel.Range).Value2?.ToString();

                    if (!string.IsNullOrEmpty(descriptionValue) && (descriptionValue.Contains("RUTW") || descriptionValue.Contains("SPXW")))
                    {
                        if (!string.IsNullOrEmpty(symbolValue))
                        {
                            // Modify Symbol column
                            if (descriptionValue.Contains("SPXW") && symbolValue.Contains("SPX") && !symbolValue.Contains("SPXW"))
                            {
                                (usedRange.Cells[i, symbolCol] as Excel.Range).Value2 = symbolValue.Replace("SPX", "SPXW");
                            }
                            else if (descriptionValue.Contains("RUTW") && symbolValue.Contains("RUT") && !symbolValue.Contains("RUTW"))
                            {
                                (usedRange.Cells[i, symbolCol] as Excel.Range).Value2 = symbolValue.Replace("RUT", "RUTW");
                            }
                        }

                        if (!string.IsNullOrEmpty(ulSymbolValue))
                        {
                            // Modify ULSymbol column
                            if (descriptionValue.Contains("SPXW") && ulSymbolValue.Contains("SPX") && !ulSymbolValue.Contains("SPXW"))
                            {
                                (usedRange.Cells[i, ulSymbolCol] as Excel.Range).Value2 = ulSymbolValue.Replace("SPX", "SPXW");
                            }
                            else if (descriptionValue.Contains("RUTW") && ulSymbolValue.Contains("RUT") && !ulSymbolValue.Contains("RUTW"))
                            {
                                (usedRange.Cells[i, ulSymbolCol] as Excel.Range).Value2 = ulSymbolValue.Replace("RUT", "RUTW");
                            }
                        }
                    }
                }

                // Save the modified workbook
                workbook.SaveAs(csvFilePath);
                Console.WriteLine($"File saved successfully: {csvFilePath}");
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed($"Error: {ex.Message}");
            }
            finally
            {
                methodCount++;
                workbook?.Close(false);
                excelApp?.Quit();
                ReleaseObject(worksheet);
                ReleaseObject(workbook);
                ReleaseObject(excelApp);
            }
        }

        public static void UnmergeCells(string filePath)
        {
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet sheet = null; // Assuming the first sheet
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(filePath);
            //outputFolder += "\\" + fileName;
            //string outputFolder = Path.Combine(Path.GetDirectoryName(filePath), "Output_Files");
            //string outputFile = Path.Combine(outputFolder, fileName);
            try
            {

                // Load Excel application
                excelApp = new Excel.Application();
                workbook = excelApp.Workbooks.Open(filePath);
                sheet = (Excel.Worksheet)workbook.Sheets[1]; // Assuming the first sheet

                excelApp.DisplayAlerts = false;
                // Extract filter criteria
                // Unmerge all merged cells in the used range
                Excel.Range usedRange = sheet.UsedRange;
                Console.WriteLine("Unmerging all merged cells...");

                foreach (Excel.Range cell in usedRange.Cells)
                {
                    if (cell.MergeCells) cell.MergeCells = false; // Unmerge if merged
                }
                Console.WriteLine("All merged cells have been unmerged.");

                // Save and close the workbook
                workbook.SaveAs(csvFilePath);
                Console.WriteLine($"File saved successfully: {csvFilePath}");
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed($"Error: {ex.Message}");
            }
            finally
            {
                methodCount++;
                // Cleanup resources
                workbook?.Close(false);
                excelApp?.Quit();
                ReleaseObject(sheet);
                ReleaseObject(workbook);
                ReleaseObject(excelApp);
            }
        }

        public static void CopyColumnValues(string filePath)
        {

            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(filePath);

            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet worksheet = workbook.ActiveSheet;
            excelApp.DisplayAlerts = false;
            string[] arr = args.Split('#');
            // Parse the arguments
            string[] parameters = args.Split('#');
            int columnFrom = int.Parse(parameters[0]);    // Column index to copy from
            int columnTo = int.Parse(parameters[1]);      // Column index to paste into
            int startRowFrom = int.Parse(parameters[2]);  // Start row index of the source column
            int startRowTo = int.Parse(parameters[3]);    // Start row index of the target column

            Excel.Range usedRange = worksheet.UsedRange;
            int totalRows = usedRange.Rows.Count;

            try
            {
                // Loop through the rows and copy values
                for (int i = 0; i <= totalRows - startRowFrom; i++)
                {
                    var sourceCellValue = (worksheet.Cells[startRowFrom + i, columnFrom] as Excel.Range)?.Value2;

                    // Paste the value only if it exists in the source column
                    if (sourceCellValue != null)
                    {
                        worksheet.Cells[startRowTo + i, columnTo].Value2 = sourceCellValue;
                    }
                }

                Console.WriteLine("Column values copied successfully.");
                // Save and close the workbook
                workbook.SaveAs(csvFilePath);
                Console.WriteLine($"File saved successfully: {csvFilePath}");
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();
            }
        }

        public static void CopyFormatToEntireSheet(string filePath)
        {

            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(filePath);

            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet worksheet = workbook.ActiveSheet;
            excelApp.DisplayAlerts = false;
            string[] arr = args.Split('#');
            // Parse the arguments
            string[] parameters = args.Split('#');
            int rowNum = int.Parse(parameters[0]);  // Row index of the source cell
            int colNum = int.Parse(parameters[1]);  // Column index of the source cell

            // Get the source cell
            Excel.Range sourceCell = worksheet.Cells[rowNum, colNum] as Excel.Range;

            // Get the entire used range of the sheet
            Excel.Range usedRange = worksheet.UsedRange;

            try
            {
                if (sourceCell != null && usedRange != null)
                {
                    // Apply the source cell format to the entire sheet
                    sourceCell.Copy();
                    usedRange.PasteSpecial(Excel.XlPasteType.xlPasteFormats);

                    Console.WriteLine("Cell format copied to the entire sheet successfully.");
                }
                else
                {
                    Console.WriteLine("Source cell or used range is invalid.");
                }

                // Clear the clipboard to avoid Excel prompt
                excelApp.CutCopyMode = 0;

                workbook.SaveAs(csvFilePath);
                Console.WriteLine($"File saved successfully: {csvFilePath}");
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
            }
            finally
            {
                methodCount++;
                workbook.Close();
                excelApp.Quit();
            }
        }

        public static void FindCellValueAtSourceColumnAndAppendValueInTargetColumn(string filePath)
        {
            try
            {
                // Parse the arguments
                string[] arr = args.Split('#');
                int sourceColumnIndex = int.Parse(arr[0]);
                int targetColumnIndex = int.Parse(arr[1]);
                string targetValue = arr[2];
                string replaceValue = arr[3];

                // Create an instance of Excel Application
                Application excelApp = new Application();
                excelApp.DisplayAlerts = false;

                // Open the Excel workbook
                Workbook workbook = excelApp.Workbooks.Open(filePath);

                // Get the worksheet
                Worksheet worksheet = (Worksheet)workbook.Sheets[1];

                // Get the used range of the worksheet
                Range usedRange = worksheet.UsedRange;

                // Get the values of the entire source column
                Range sourceColumnRange = usedRange.Columns[sourceColumnIndex];
                Range targetColumnRange = usedRange.Columns[targetColumnIndex];

                // Get the cell values as arrays
                object[,] sourceValues = (object[,])sourceColumnRange.Value;
                object[,] targetValues = (object[,])targetColumnRange.Value;

                // Loop through each cell in the source column
                for (int i = 1; i <= sourceValues.GetLength(0); i++)
                {
                    // Check if the cell value in the source column contains the target value
                    if (sourceValues[i, 1] != null && sourceValues[i, 1].ToString().Contains(targetValue))
                    {
                        // Get the existing value in the target column
                        string existingValue = targetValues[i, 1]?.ToString() ?? string.Empty;

                        // Append the replace value after the target value
                        targetValues[i, 1] = existingValue + replaceValue;
                    }
                }

                // Update the target column range with the new values
                targetColumnRange.Value = targetValues;

                // Prepare file paths and save the workbook
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);

                if (!string.IsNullOrEmpty(newfilename))
                {
                    fname = newfilename;
                }

                string csvFilePath = Path.Combine(outputFolder, fname + extension);
                workbook.SaveAs(csvFilePath);
                workbook.Close();

                // Quit the Excel application
                excelApp.Quit();

                // Clean up
                ReleaseObject(worksheet);
                ReleaseObject(workbook);
                ReleaseObject(excelApp);
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
        }

        public static void AppendDataToFile_V2(string FilePath)
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fname = Path.GetFileNameWithoutExtension(FilePath);
            string extension = Path.GetExtension(FilePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string[] arr = args.Split('#');
            string partialFilePath = currentDirectory;
            string partialFileName = arr[1];
            int startRow = int.Parse(arr[2]);

            if (!arr[0].Equals("NA"))
            {
                partialFilePath = Path.Combine(currentDirectory, arr[0]);
            }

            string[] matchingFiles = FindFilesWithPartialName(partialFilePath, partialFileName);
            string otherFilePath = matchingFiles.Length > 0 ? matchingFiles[0] : null;

            // Check if main file exists
            if (!File.Exists(FilePath))
            {
                Console.WriteLine("Main file does not exist. Creating a new file.");
                LogWithRed("Main file does not exist. Creating a new file.");
                File.Create(FilePath).Dispose();
            }

            // Check if other file exists
            if (otherFilePath == null || !File.Exists(otherFilePath))
            {
                Console.WriteLine("Other file does not exist. Proceeding with main file only.");
                LogWithRed("Other file does not exist. Proceeding with main file only.");
                otherFilePath = null; // Mark as null to handle logic for a single file
            }

            // Create Excel Application instance
            Excel.Application excelApp = new Excel.Application();
            excelApp.ScreenUpdating = false;

            Excel.Workbook mainWorkbook = null;
            Excel.Workbook otherWorkbook = null;
            Excel.Worksheet mainSheet = null;
            Excel.Worksheet otherSheet = null;

            try
            {
                // Open main workbook
                mainWorkbook = excelApp.Workbooks.Open(FilePath);
                mainSheet = mainWorkbook.Sheets[1];

                if (otherFilePath != null)
                {
                    // Open other workbook
                    otherWorkbook = excelApp.Workbooks.Open(otherFilePath);
                    otherSheet = otherWorkbook.Sheets[1];

                    // Get last row in other worksheet
                    int lastRow = otherSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;

                    // Iterate through rows in other file starting from startRow
                    for (int rowIndex = startRow; rowIndex <= lastRow; rowIndex++)
                    {
                        // Check if the row is empty
                        if (otherSheet.Cells[rowIndex, 1].Value != null)
                        {
                            // Get row from other worksheet
                            Excel.Range otherRow = otherSheet.Rows[rowIndex];

                            // Copy row to main worksheet
                            otherRow.Copy(mainSheet.Rows[mainSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row + 1]);
                        }
                    }
                }

                // Save changes to main workbook
                mainWorkbook.SaveAs(csvFilePath);

                Console.WriteLine("Data appended to main file at: " + csvFilePath);
                CreateLogs("Data appended to main file at: " + csvFilePath, logFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                LogWithRed("An error occurred: " + ex.Message);
            }
            finally
            {
                methodCount++;
                // Close and release resources
                if (mainWorkbook != null)
                {
                    mainWorkbook.Close(false);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(mainWorkbook);
                }
                if (otherWorkbook != null)
                {
                    otherWorkbook.Close(false);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(otherWorkbook);
                }
                if (excelApp != null)
                {
                    excelApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                }
                // Release COM objects
                if (mainWorkbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(mainWorkbook);
                if (mainSheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(mainSheet);
                if (otherWorkbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(otherWorkbook);
                if (otherSheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(otherSheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }

        }
        public static void FilterAndReplaceInOtherColumn_SSOMS(string filePath)
        {
            try
            {
                string[] arr = args.Split('#');
                int filterColumnNumber = int.Parse(arr[0]);
                string targetValue = arr[1];
                int replaceColumnNumber = int.Parse(arr[2]);

                Excel.Application excelApp = new Excel.Application();
                excelApp.DisplayAlerts = false;
                Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
                Excel.Worksheet worksheet = workbook.Sheets[1];
                Excel.Range usedRange = worksheet.UsedRange;
                int rowCount = usedRange.Rows.Count;

                object lastNonEmptyValue = null;
                Dictionary<int, object> replacements = new Dictionary<int, object>();

                // Iterate through each row in the used range
                for (int row = 1; row <= rowCount; row++)
                {
                    Excel.Range filterCell = usedRange.Cells[row, filterColumnNumber];
                    Excel.Range replaceCell = usedRange.Cells[row, replaceColumnNumber];

                    object filterValue = filterCell.Value2;
                    object replaceValue = replaceCell.Value2;

                    // Update last non-empty value from replace column
                    if (replaceValue != null && !string.IsNullOrWhiteSpace(replaceValue.ToString()))
                    {
                        lastNonEmptyValue = replaceValue;
                    }

                    // If the filter column cell matches targetValue, store replacement in dictionary
                    if ((filterValue != null && string.Equals(filterValue.ToString(), targetValue)) ||
                        (filterValue == null && targetValue == ""))
                    {
                        if (lastNonEmptyValue != null)
                        {
                            replacements[row] = lastNonEmptyValue;
                        }
                    }
                }

                // Apply replacements in batch (reduces Excel COM interactions)
                foreach (var entry in replacements)
                {
                    usedRange.Cells[entry.Key, replaceColumnNumber].Value = entry.Value;
                }

                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (!string.IsNullOrEmpty(newfilename))
                {
                    fname = newfilename;
                }

                string csvFilePath = Path.Combine(outputFolder, fname + extension);

                // Save and close Excel
                workbook.SaveAs(csvFilePath);
                workbook.Close(false);
                excelApp.Quit();

                ReleaseObject(worksheet);
                ReleaseObject(workbook);
                ReleaseObject(excelApp);
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message);
            }
        }

        public static void FilterAndReplaceInOtherColumn_V2(string filePath)
        {
            try
            {
                string[] arr = args.Split('#');
                int filterColumnNumber = int.Parse(arr[0]);
                object targetValue = arr[1];
                int replaceColumnNumber = int.Parse(arr[2]);
                string replacementValue = arr[3];

                Excel.Application excelApp = new Excel.Application();
                excelApp.DisplayAlerts = false;
                Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
                Excel.Worksheet worksheet = workbook.Sheets[1];
                Excel.Range usedRange = worksheet.UsedRange;
                Excel.Range filterColumn = usedRange.Columns[filterColumnNumber];
                Excel.Range replaceColumn = usedRange.Columns[replaceColumnNumber];

                if (filterColumn != null && replaceColumn != null)
                {
                    // Convert targetValue to string
                    string targetValueString = Convert.ToString(targetValue);

                    // Loop through each cell in the filter column
                    foreach (Excel.Range cell in filterColumn.Cells)
                    {
                        if (cell.Value2 != null && cell.Value2.ToString() == targetValueString)
                        {
                            // Get the corresponding cell in the replace column and replace its value
                            Excel.Range replaceCell = (Excel.Range)replaceColumn.Cells[cell.Row - filterColumn.Row + 1];
                            replaceCell.Value = replacementValue;
                        }
                        if (cell.Value2 == null && targetValueString == "")
                        {
                            // Get the corresponding cell in the replace column and replace its value
                            Excel.Range replaceCell = (Excel.Range)replaceColumn.Cells[cell.Row - filterColumn.Row + 1];
                            replaceCell.Value = replacementValue;
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Filter column number '{filterColumnNumber}' or replace column number '{replaceColumnNumber}' is invalid.");
                }


                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (newfilename != "")
                {
                    fname = newfilename;
                }

                string csvFilePath = outputFolder + @"\" + fname + extension;

                // Save and close Excel
                workbook.SaveAs(csvFilePath);
                workbook.Close();
                excelApp.Quit();

                ReleaseObject(worksheet);
                ReleaseObject(workbook);
                ReleaseObject(excelApp);
                methodCount++;
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed(ex.Message.ToString());
            }
        }

        static void ConvertCellsToNumberFormat(string filePath)
        {
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                // Split the args variable to get the parameters
                string[] arr = args.Split('#');
                string row1Str = arr[0];
                string col1Str = arr[1];
                string row2Str = arr[2];
                string col2Str = arr[3];

                // Create a new Excel application
                excelApp = new Excel.Application();
                excelApp.DisplayAlerts = false;

                // Open the workbook
                workbook = excelApp.Workbooks.Open(filePath);

                // Get the first worksheet
                worksheet = workbook.Sheets[1]; // Index is 1-based

                // Get the last row and column numbers
                int lastRow = worksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
                int lastColumn = worksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Column;

                // Parse the row and column values, or set to last row/column if null or empty
                int row1 = string.IsNullOrEmpty(row1Str) ? lastRow : int.Parse(row1Str);
                int col1 = string.IsNullOrEmpty(col1Str) ? lastColumn : int.Parse(col1Str);
                int row2 = string.IsNullOrEmpty(row2Str) ? lastRow : int.Parse(row2Str);
                int col2 = string.IsNullOrEmpty(col2Str) ? lastColumn : int.Parse(col2Str);

                // Define the range based on input arguments
                Excel.Range range = worksheet.Range[
                    worksheet.Cells[row1, col1],
                    worksheet.Cells[row2, col2]
                ];

                // Read the range values into a 2D array for processing
                object[,] data = range.Value2;

                // Loop through the array and convert cells to numeric if possible
                for (int row = 1; row <= data.GetLength(0); row++)
                {
                    for (int col = 1; col <= data.GetLength(1); col++)
                    {
                        if (data[row, col] != null &&
                            double.TryParse(data[row, col].ToString(), out double numericValue))
                        {
                            // Replace with the numeric value
                            data[row, col] = numericValue;
                        }
                    }
                }

                // Write the modified data back to the worksheet
                range.Value2 = data;


                // Save the workbook with the same or new filename
                string fname = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (!string.IsNullOrEmpty(newfilename))
                {
                    fname = newfilename;
                }
                string csvFilePath = Path.Combine(outputFolder, fname + extension);

                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                // Handle exceptions
                LogWithRed("An error occurred: " + ex.Message);
            }
            finally
            {
                methodCount++;
                // Close workbook and Excel application properly
                if (workbook != null)
                {
                    workbook.Close(false);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                }
                if (excelApp != null)
                {
                    excelApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                }
                if (worksheet != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                }

                // Force garbage collection to release resources
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        #region UpdateValueAccordingToCondition 

        // Function to apply the condition based on the string expression
        public static void UpdateValueAccordingToCondition(string filePath)
        {
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string csvFilePath = outputFolder + @"\" + fname + ".csv";

            // Initialize Excel application
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[1];
            excelApp.DisplayAlerts = false;

            var conditions = args.Split(';');

            try
            {
                // Get the header row (first row)
                Excel.Range headerRange = sheet.Rows[1];
                // int columnCount = headerRange.Columns.Count;
                int columnCount = sheet.UsedRange.Columns.Count;

                Dictionary<string, int> columnIndexes = new Dictionary<string, int>();

                // Store column names and their indexes
                for (int col = 1; col <= columnCount; col++)
                {
                    string columnName = headerRange.Cells[1, col].Text.ToString();
                    columnIndexes[columnName] = col;
                }

                // Iterate through all rows starting from the second row
                for (int row = 2; row <= sheet.UsedRange.Rows.Count; row++)
                {
                    bool rowUpdated = false;

                    // Evaluate each condition
                    foreach (var condition in conditions)
                    {
                        // Parse the condition string
                        var parts = condition.Split(new string[] { " => " }, StringSplitOptions.None);
                        if (parts.Length == 2)
                        {
                            string conditionPart = parts[0];
                            string actionPart = parts[1];

                            // Parse the action part to get the column and value to update
                            var actionParts = actionPart.Split(new string[] { " = " }, StringSplitOptions.None);
                            if (actionParts.Length == 2)
                            {
                                string columnToUpdate = actionParts[0].Trim();
                                string newValue = actionParts[1].Trim().Trim('\'');

                                // Evaluate the condition for this row
                                bool conditionMet = EvaluateCondition(sheet, row, conditionPart, columnIndexes);

                                if (conditionMet && sheet.Cells[row, columnIndexes[columnToUpdate]].Value != newValue)
                                {
                                    // Apply the update if condition is met
                                    sheet.Cells[row, columnIndexes[columnToUpdate]].Value = newValue;
                                    rowUpdated = true;
                                }
                            }
                        }
                    }
                }

                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                workbook.Close();
                excelApp.Quit();

                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (sheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        // Method to evaluate the condition for a row
        private static bool EvaluateCondition(Worksheet sheet, int row, string conditionPart, Dictionary<string, int> columnIndexes)
        {
            // Split conditions by logical AND (&&)
            var conditions = conditionPart.Split(new string[] { "&&" }, StringSplitOptions.None);

            bool result = true;

            foreach (var condition in conditions)
            {
                // Trim any whitespace and parse the column name and expected value
                string trimmedCondition = condition.Trim();
                var conditionParts = trimmedCondition.Split(new string[] { "==" }, StringSplitOptions.None);

                if (conditionParts.Length == 2)
                {
                    string columnName = conditionParts[0].Trim();
                    string expectedValue = conditionParts[1].Trim().Trim('\'');

                    // Get the value from the specified column and row
                    string cellValue = sheet.Cells[row, columnIndexes[columnName]].Text.ToString().Trim();

                    // Check if the cell value matches the expected value
                    if (cellValue?.ToLower() != expectedValue?.ToLower())
                    {
                        result = false;
                        break; // If any condition fails, stop checking
                    }
                }
            }

            return result;
        }

        #endregion UpdateValueAccordingToCondition 

        public static void UpdateColumnValueForTicker(string filePath)
        {
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string csvFilePath = outputFolder + "\\" + fname + ".csv";

            Application excelApp = new Application();
            excelApp.DisplayAlerts = false;
            Workbook workbook = excelApp.Workbooks.Open(filePath);
            Worksheet worksheet = (Worksheet)workbook.Sheets[1];

            try
            {
                string[] arr = args.Split('#');
                string targetColumnName = arr[0];
                string replaceColumnName = arr[1];
                string dbValueName = arr[2];

                int targetColumnIndex = FindColumnIndex(worksheet, targetColumnName);
                int replaceColumnIndex = FindColumnIndex(worksheet, replaceColumnName);

                if (targetColumnIndex == -1 || replaceColumnIndex == -1)
                {
                    throw new Exception("One or more specified column names not found in the Excel sheet.");
                }

                Range usedRange = worksheet.UsedRange;
                int rowCount = usedRange.Rows.Count;

                // Collect all tickers first
                HashSet<string> tickers = new HashSet<string>();
                for (int row = 2; row <= rowCount; row++)
                {
                    Range cell = (Range)worksheet.Cells[row, replaceColumnIndex];
                    string tickerSymbol = cell.Value?.ToString()?.Trim();
                    if (!string.IsNullOrEmpty(tickerSymbol))
                    {
                        tickers.Add(tickerSymbol);
                    }
                }

                // Fetch all ticker data in one database call
                var tickerData = GetBulkDataForTickers(tickers);

                var property = typeof(T_SMSymbolLookUpTable).GetProperty(dbValueName);
                if (property == null)
                {
                    throw new Exception($"Property '{dbValueName}' not found in database entity.");
                }

                // Update values in Excel
                for (int row = 2; row <= rowCount; row++)
                {
                    Range cell = (Range)worksheet.Cells[row, replaceColumnIndex];
                    string tickerSymbol = cell.Value?.ToString()?.Trim();

                    if (!string.IsNullOrEmpty(tickerSymbol) && tickerData.TryGetValue(tickerSymbol, out var entity))
                    {
                        object dbValue = property.GetValue(entity);
                        if (dbValue != null)
                        {
                            var value = dbValue.ToString().Trim();

                            // Todo: use separate function
                            if (dbValueName == "CUSIPSymbol")
                            {
                                value = "'" + value.PadLeft(9, '0');
                            }

                            // worksheet.Cells[row, targetColumnIndex].Value = "'" + value;
                            worksheet.Cells[row, targetColumnIndex].Value = value;
                        }
                    }
                    else
                    {
                        exitcode = 3;
                        string errorMessage = $"CUSIP is not found for ticker {tickerSymbol}, Please add it in SM and update it manually in file.";
                        LogWithRed(errorMessage);
                        CreateLogs(errorMessage, logFilePath);
                    }
                }

                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                exitcode = 3;
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                workbook.Close();
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        // Helper method to find column index by name
        private static int FindColumnIndex(Worksheet worksheet, string columnName)
        {
            Range usedRange = worksheet.UsedRange;
            int colCount = usedRange.Columns.Count;

            for (int col = 1; col <= colCount; col++)
            {
                string header = ((Range)worksheet.Cells[1, col]).Value?.ToString()?.Trim();
                if (header == columnName)
                {
                    return col;
                }
            }

            return -1; // Column not found
        }

        private static Dictionary<string, T_SMSymbolLookUpTable> GetBulkDataForTickers(IEnumerable<string> tickers)
        {
            using (var context = new SMDbContext())
            {
                // This method should make a single database call to fetch all required ticker data at once
                return context.T_SMSymbolLookUpTable
                .Where(t => tickers.Contains(t.TickerSymbol))
                .ToDictionary(t => t.TickerSymbol, t => t);
            }
        }

        private static T_SMSymbolLookUpTable GetDataForTicker(string ticker)
        {
            using (var context = new SMDbContext())
            {
                var entity = context.T_SMSymbolLookUpTable.FirstOrDefault(e => e.TickerSymbol == ticker);

                if (entity != null)
                {
                    return entity;
                }
                else
                {
                    throw new Exception($"No record exist in DB for {ticker}");
                }
            }
        }

        static void RearrangeCsvColumns(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            if (lines.Length == 0)
            {
                Console.WriteLine("CSV file is empty.");
                return;
            }

            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(filePath);

            string[] arr = args.Split('#');
            string headerTexts = arr[0];
            string[] desiredOrder = headerTexts.Split(';');

            // Read header and determine column indices
            string[] originalHeader = lines[0].Split(',');
            Dictionary<string, int> columnIndexMap = originalHeader.Select((col, index) => new { col, index })
                                                                   .ToDictionary(x => x.col, x => x.index);

            // Create new header ensuring all desired columns exist
            string[] newHeader = desiredOrder;

            // Rearrange rows based on new header order
            List<string> newLines = new List<string> { string.Join(",", newHeader) };

            for (int i = 1; i < lines.Length; i++)
            {
                string[] rowValues = lines[i].Split(',');
                string[] newRow = new string[newHeader.Length];

                for (int j = 0; j < newHeader.Length; j++)
                {
                    if (columnIndexMap.ContainsKey(newHeader[j]))
                    {
                        int originalIndex = columnIndexMap[newHeader[j]];
                        newRow[j] = originalIndex < rowValues.Length ? rowValues[originalIndex] : "";
                    }
                    else
                    {
                        newRow[j] = ""; // If column not in original CSV, add empty column
                    }
                }

                newLines.Add(string.Join(",", newRow));
            }

            // Write the new CSV file
            File.WriteAllLines(csvFilePath, newLines);
        }



        public static List<DateTime> GetBusinessDates(int daysBack, List<DateTime> holidays)
        {
            List<DateTime> businessDates = new List<DateTime>();
            DateTime currentDate = processDate;
            int daysCounted = 0;

            while (daysCounted < daysBack)
            {

                // Check if it's a weekend (Saturday or Sunday)
                if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    currentDate = currentDate.AddDays(-1);
                    continue;
                }

                // Check if it's a holiday
                if (holidays.Contains(currentDate.Date))
                {
                    currentDate = currentDate.AddDays(-1);
                    continue;
                }

                // Add valid business day to the list
                businessDates.Add(currentDate);
                daysCounted++;
                currentDate = currentDate.AddDays(-1);
            }

            return businessDates;
        }
        static List<DateTime> ReadHolidaysFromFile(string filePath)
        {
            List<DateTime> holidays = new List<DateTime>();

            try
            {
                foreach (var line in File.ReadLines(filePath))
                {
                    // Parse each line as a DateTime (assuming the format is "yyyy-MM-dd")
                    if (DateTime.TryParse(line, out DateTime holiday))
                    {
                        holidays.Add(holiday);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading holidays from file: " + ex.Message);
            }

            return holidays;
        }

        // Method to filter rows based on dates from today to 30 business days ago
        public static void deleteRowsByDateRange(string filePath)
        {
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            if (newfilename != "")
            {
                fname = newfilename;
            }

            string csvFilePath = outputFolder + @"\" + fname + extension;
            string fileName = Path.GetFileName(filePath);

            // Load Excel application
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[1]; // Assuming the first sheet

            excelApp.DisplayAlerts = false;

            // Filter criteria from args (modify as needed)
            //string args = "1#Header#2#30"; // Example args
            string[] arr = args.Split('#');
            int columnIndex = int.Parse(arr[0]);
            string headerText = arr[1];
            int startRowIndex = int.Parse(arr[2]);
            int daysToCount = int.Parse(arr[3]);

            // Read holidays from the holidays.txt file
            List<DateTime> holidays = ReadHolidaysFromFile(Directory.GetCurrentDirectory() + @"\holidays.txt");
            // Get list of valid business days (from today to 30 days before)
            List<DateTime> businessDates = GetBusinessDates(daysToCount, holidays);
            // Filter and delete rows
            Excel.Range usedRange = sheet.UsedRange;
            int rowCount = usedRange.Rows.Count;

            try
            {
                for (int rowIndex = startRowIndex; rowIndex <= rowCount; rowIndex++)
                {
                    Excel.Range cell = (Excel.Range)sheet.Cells[rowIndex, columnIndex];
                    string cellContent = cell.Value != null ? cell.Value.ToString() : "";

                    DateTime cellDate;
                    // Check if the date in the cell is in the business dates list
                    string[] formats = {
            "yyyyMMdd", "MM/dd/yyyy", "M/d/yyyy", "dd-MM-yyyy", "d-M-yyyy",
            "yyyy-MM-dd", "MM-dd-yyyy", "dd/MM/yyyy", "d/M/yyyy", "M/d/yy",
            "MM/dd/yy", "dd.MM.yyyy", "yyyy/MM/dd", "MMM dd, yyyy",
            "dd MMM yyyy", "d MMMM yyyy", "MMM d, yyyy", "yyyyMMddHHmmss"
        };

                    bool isDate = DateTime.TryParseExact(
                        cellContent,
                        formats,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out cellDate
                    );

                    if (isDate && !businessDates.Contains(cellDate.Date))
                    {
                        Excel.Range rowToDelete = (Excel.Range)sheet.Rows[rowIndex];
                        rowToDelete.Delete();
                        rowIndex--; // Adjust rowIndex since the next row moves up after deletion
                        rowCount--; // Adjust rowCount after deletion
                    }
                }

                // Save changes
                workbook.SaveAs(csvFilePath);
                Console.WriteLine("Rows not containing valid business dates have been deleted.");
            }
            catch (Exception ex)
            {
                LogWithRed("Error: " + ex.Message);
                CreateLogs("Error: " + ex.Message, logFilePath);
            }
            finally
            {
                workbook.Close();
                excelApp.Quit();
                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (sheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }
    }
}
