using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System;
using Excel = Microsoft.Office.Interop.Excel;
//using Microsoft.Office.Interop.Excel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

using System.Linq;
using System.Data;
//using OfficeOpenXml;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using NPOI.SS.Formula.Functions;

namespace ConsoleApplicatiog
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
        private static string logFilename;
        static string inputFolder = "";
        static string outputFolder = "";
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
        public static DateTime processDate = GetProcessDate(Directory.GetCurrentDirectory() + @"\Config\ProcessDateXmlPath.txt");

        static Dictionary<string, string> nameconfig = new Dictionary<string, string>();

        static void Main(string[] args)
        {
            InitializeLogFile();
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
                Console.WriteLine("Config file not found: " + mainfilePath);
            }



            Console.WriteLine("Conversion Completed");
            //Console.ReadLine();
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
                                            Console.WriteLine("Value of last cell in the row: " + lastCell.Value);

                                            break; // Exit the loop once the value is found
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Cell with value 'Strategy Name' not found.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Worksheet not found.");
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
                    Log("Conversion to CSV completed for " + fname);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    Log("Error: " + ex.Message);
                }

                //clear format

                //}


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Log("Error: " + ex.Message);
            }
            finally
            {
                workbook.Close(false);
                excelApp.Quit();
            }
        }
        static void ProcessConfiguration(string configFile)
        {
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //using (var package = new ExcelPackage(new FileInfo(configFile)))
            //{
            //    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

            //    int rowCount = worksheet.Dimension.Rows;

            //    for (int row = 2; row <= rowCount; row++)
            //    {
            //        string inputFolder = worksheet.Cells[row, 1].Text;
            //        string outputFolder = worksheet.Cells[row, 2].Text;
            //        string functionName = worksheet.Cells[row, 3].Text;
            //        string individualFile = worksheet.Cells[row, 4].Text;
            //        string fileToFunction = worksheet.Cells[row, 5].Text;
            //        string havetoappend = worksheet.Cells[row, 6].Text;

            //        if (individualFile.ToLower() == "true")
            //        {
            //            // Process individual file using fileToFunction
            //            ProcessIndividualFile(inputFolder, outputFolder, fileToFunction);
            //        }
            //        else
            //        {
            //            // Process all files in inputFolder using functionName
            //            ProcessAllFiles(inputFolder, outputFolder, functionName);
            //        }
            //    }
            //}
            System.Data.DataTable mainFileData = DataFromCsvFile(configFile);

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
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error extracting files: " + ex.Message);
            }
        }
        public static void DeleteRow(Excel.Worksheet worksheet, int rowNumber)
        {
            Excel.Range rowToDelete = worksheet.Rows[rowNumber];

            Console.WriteLine("Cell Value: " + worksheet.Cells[rowNumber, 1].Value);
            rowToDelete.Delete();
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
                    }
                }

                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
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
                Console.WriteLine("Error: " + ex.Message);
                Log("Error: " + ex.Message);
            }
            finally
            {
                workbook.Close(false);
                excelApp.Quit();
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
                Console.WriteLine("Error: " + ex.Message);
                Log("Error: " + ex.Message);
            }
            finally
            {
                workbook.Close(false);
                excelApp.Quit();
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
                Console.WriteLine("Error: " + ex.Message);
                Log("Error: " + ex.Message);
            }
            finally
            {
                workbook.Close(false);
                excelApp.Quit();
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
                Console.WriteLine("Error: " + ex.Message);
                Log("Error: " + ex.Message);
            }

        }
        static void MoveScriptFile(string sourceFilePath, string destinationFolderPath, string scriptFileName)
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
            }
            else
            {
                Console.WriteLine("Source script file does not exist.");
            }
        }


        static void RunScript(string scriptFilePath)
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

        static void MoveFileToNewFolder(string sourceFilePath, string destinationFilePath)
        {
            if (File.Exists(sourceFilePath))
            {
                // Directory.CreateDirectory(Path.GetDirectoryName(destinationFilePath));
                if (!File.Exists(destinationFilePath))
                {
                    File.Move(sourceFilePath, destinationFilePath);
                }

                Console.WriteLine("File moved successfully!");
            }
            else
            {
                Console.WriteLine("Source file does not exist.");
            }
        }
        static void MoveFiles(string sourceFilePath)
        {
            string destinationFilePath = outputFolder;

            if (File.Exists(sourceFilePath))
            {
                // Directory.CreateDirectory(Path.GetDirectoryName(destinationFilePath));
                if (!File.Exists(destinationFilePath))
                {
                    File.Copy(sourceFilePath, destinationFilePath);
                }

                Console.WriteLine("File moved successfully!");
            }
            else
            {
                Console.WriteLine("Source file does not exist.");
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
                // Handle any exceptions that may occur during file reading
                Console.WriteLine("An error occurred while reading the CSV file: " + ex.Message);
            }

            return dataTable;
        }

        static void ProcessIndividualFile(string inputFolder, string outputFolder, string fileToFunction)
        {
            Dictionary<string, List<Action<string>>> keywordFunctions = ReadConfig(fileToFunction);

            IterateFilesInFolder(inputFolder, keywordFunctions, outputFolder);

            Console.WriteLine($"Processing individual file: {fileToFunction} from {inputFolder} to {outputFolder}");
        }

        static void ProcessAllFiles(string inputFolder, string outputFolder, string functionName)
        {
            string[] pairs = functionName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string pair in pairs)
            {
                Action<string> function = GetFunctionByName(functionName);
                IterateAllFilesInFolder(inputFolder, function, outputFolder);
            }

            // Action<string> function= GetFunctionByName(functionName);
            //IterateAllFilesInFolder(inputFolder, function, outputFolder);
            Console.WriteLine($"Processing all files in {inputFolder} using function: {functionName} to {outputFolder}");
        }
        static Dictionary<string, List<Action<string>>> ReadConfig(string configString)
        {
            Dictionary<string, List<Action<string>>> keywordFunctions = new Dictionary<string, List<Action<string>>>();

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

            return keywordFunctions;
        }
        static Dictionary<string, string> ReadConfignames(string configString)
        {
            Dictionary<string, string> keywordFunctions = new Dictionary<string, string>();

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
            }
            catch (Exception ex)
            {
                // If there is an error, display the error message
                Console.WriteLine("Error creating log file: " + ex.Message);
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
                Console.WriteLine("Error writing to log file: " + ex.Message);
                Log("Config file does not exist.");
            }
        }


        static Action<string> GetFunctionByName(string functionName)
        {
            // functionName=functionName.ToLower();
            // Implement a mapping from function names to corresponding functions
            switch (functionName)
            {
                case "ConvertExcelToxls":
                    return ConvertExcelToxls;
                case "ConvertExcelToCSV":
                    return ConvertExcelToCSV;
                case "ConvertexttoCol":
                    return ConvertexttoCol;
                case "clearformat":
                    return ClearFormat;
                case "ClearFormatParticularColumn":
                    return ClearFormatParticularColumn;
                case "changeextension":
                    return changeextension;
                case "copyfile":
                    return Copyfile;
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
                case "ConvertColValueToTwoDecimals":
                    return ConvertColValueToTwoDecimals;
                case "FindCellValueAndReplace":
                    return FindCellValueAndReplace;
                case "MergeCsvFilesToExcel":
                    return MergeCsvFilesToExcel;
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
                case "LW_JPM_UpdateDataFromOtherFile":
                    return LW_JPM_UpdateDataFromOtherFile;
                case "FilterAndPerformSubstraction":
                    return FilterAndPerformSubstraction;
                case "CopyColumnValuesBasedOnCriteria":
                    return CopyColumnValuesBasedOnCriteria;
                case "SubtractAndPasteValues":
                    return SubtractAndPasteValues;
                case "AlignColumnData":
                    return AlignColumnData;
                case "DeleteColumnsWithHeaderTextFromExcel":
                    return DeleteColumnsWithHeaderTextFromExcel;
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
                case "SortRowsByColumn":
                    return SortRowsByColumn;



                //case "changefile":
                //    return changefile;
                default:
                    return null;
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
            if(isAscending == "asc")
            {
                ascending = true;
            }

            try
            {
                workbook = excelApp.Workbooks.Open(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error opening Excel file: " + ex.Message);
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
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                workbook.Close(false);
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
                Console.WriteLine("Error: " + ex.Message);
                Log("Error: " + ex.Message);
            }
            finally
            {
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
                Console.WriteLine("An error occurred: " + ex.Message);
                Console.WriteLine(ex.StackTrace);

            }

            finally
            {
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
                Console.WriteLine("An error occurred: " + ex.Message);
                Console.WriteLine(ex.StackTrace);

            }

            finally
            {
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
                Console.WriteLine("An error occurred: " + ex.Message);
                Console.WriteLine(ex.StackTrace);

            }

            finally
            {
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
                Console.WriteLine("Error opening Excel file: " + ex.Message);
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
                Console.WriteLine("Error: " + ex.Message);
                Log("Error: " + ex.Message);
            }
            finally
            {
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
                Console.WriteLine("Error opening file: " + ex.Message);
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
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                workbook.Close(false);
                excelApp.Quit();

                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
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
            finally
            {
                // Cleanup
                workbook?.Close(false);
                excelApp.Quit();

                // Release COM objects
                if (worksheet != null) Marshal.ReleaseComObject(worksheet);
                if (workbook != null) Marshal.ReleaseComObject(workbook);
                Marshal.ReleaseComObject(excelApp);
            }
        }

        public static void DeleteColumnsWithHeaderText(Excel.Worksheet worksheet, string headerText, string headerRowIndex)
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
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            finally
            {
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
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            finally
            {
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
                    Marshal.ReleaseComObject(workbook);
                }

                // Quit the Excel application
                excelApp.Quit();
                Marshal.ReleaseComObject(excelApp);
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
                Console.WriteLine("An error occurred: " + ex.Message);
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
                Console.WriteLine("An error occurred: " + ex.Message);
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
                Console.WriteLine("Error: " + ex.Message);
                return -1;
            }
            finally
            {
                // Clean up
                if (workbook != null) workbook.Close(false);
                if (workbooks != null) workbooks.Close();
                if (excelApp != null) excelApp.Quit();


            }
        }


        public static void DeletePdfFiles(string filepath)
        {
            string directoryPath = inputFolder;
            try
            {
                // Check if directory exists
                if (!Directory.Exists(directoryPath))
                {
                    Console.WriteLine("Directory does not exist.");
                    return;
                }

                // Get all PDF files in the directory
                string[] pdfFiles = Directory.GetFiles(directoryPath, "*.pdf");

                // Delete each PDF file
                foreach (string pdfFile in pdfFiles)
                {
                    File.Delete(pdfFile);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        public static void CutPdfFiles(string filepath )
        {
            string sourceDirectory=inputFolder;
            string destinationDirectory= outputFolder;
            try
            {
                // Check if source directory exists
                if (!Directory.Exists(sourceDirectory))
                {
                    Console.WriteLine("Source directory does not exist.");
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
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
            workbook.Close();
            excelApp.Quit();

            Console.WriteLine("Rows not containing the specified text in the specified column have been deleted.");


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
                return;
            }

            if (!File.Exists(otherFilePath))
            {
                Console.WriteLine("Other file does not exist.");
                return;
            }

            // Create Excel Application instance
            Excel.Application excelApp = new Excel.Application
            {
                ScreenUpdating = false
            };

            try
            {
                // Open main workbook
                Excel.Workbook mainWorkbook = excelApp.Workbooks.Open(mainFilePath);
                // Open other workbook
                Excel.Workbook otherWorkbook = excelApp.Workbooks.Open(otherFilePath);

                // Get main worksheet
                Excel.Worksheet mainSheet = mainWorkbook.Sheets[1]; // Assuming data is in the first worksheet

                // Get other worksheet
                Excel.Worksheet otherSheet = otherWorkbook.Sheets[1]; // Assuming data is in the first worksheet

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
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            finally
            {
                
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        public static void ClearLeadingTrailingEmptyStrings(string filePath)
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
        }
        public static void CopyRowFromOneFileToOther(string destinationFilePath)
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
            sourceWorkbook.Close(false);
            destinationWorkbook.Close(true);

            // Release COM objects
            System.Runtime.InteropServices.Marshal.ReleaseComObject(destinationRange);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(sourceRange);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(destinationWorksheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(sourceWorksheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(destinationWorkbook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(sourceWorkbook);
            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
        }


        static void SetCellValue(string filePath)
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
                }
                else
                {
                    Console.WriteLine("Directory does not exist.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        public static void PerformDivisionBetweenTwoColumn(string filePath)
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
        }



        public static void InsertBlankColumn(string filePath)
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
        }

        public static void InsertBlankRow(string filePath)
        {

            string[] arr = args.Split('#');
            int rowIndex = int.Parse(arr[0]);
            Excel.Application excelApp = new Excel.Application();
            excelApp.DisplayAlerts = false;
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.ActiveSheet;

            // Shift cells down to make room for the new row
            Excel.Range range = worksheet.Rows[rowIndex];
            range.Insert(Excel.XlInsertShiftDirection.xlShiftDown, Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);

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
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
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
                        Console.WriteLine("Failed to extract FilterValue.");
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return DateTime.MinValue;
        }

        public static string ReplaceDateTimePlaceholders(string input)
        {
            string pattern = @"\(([^)]+)\)";
            Regex regex = new Regex(pattern);

            MatchCollection matches = regex.Matches(input);

            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                string placeholder = match.Groups[1].Value;
                string replacement = GetFormattedDateTime(placeholder);
                input = input.Replace(match.Value, replacement);
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
                Console.WriteLine($"Error reading XML path from config: {ex.Message}");
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
            int endIndex = int.Parse(arr[1]);
            Excel.Range usedRange = worksheet.UsedRange;

            try
            {
                // Ensure start index and end index are within the range of rows in the worksheet
                if (startIndex >= 1 && endIndex <= usedRange.Rows.Count && startIndex <= endIndex)
                {
                    // Delete rows starting from endIndex and work backwards to startIndex
                    for (int rowIndex = endIndex; rowIndex >= startIndex; rowIndex--)
                    {
                        Excel.Range row = (Excel.Range)worksheet.Rows[rowIndex];
                        row.Delete();
                    }

                    workbook.SaveAs(csvFilePath);
                }
                else
                {
                    Console.WriteLine("Invalid start index or end index.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                workbook.Close();
                excelApp.Quit();

                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
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
                Console.WriteLine("Error: " + ex.Message);
                Log("Error: " + ex.Message);
            }
            finally
            {
                workbook.Close(false);
                excelApp.Quit();
            }
        }
        public static void ConvertColValuetoNumber(string filePath)
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

                //int columnNumber = -1;
                //columnNumber= int.Parse(args);
                foreach (string col in columns)
                {
                    if (int.TryParse(col, out int columnNumber) && columnNumber > 0)
                    {
                        Excel.Range columnRange = worksheet.Columns[columnNumber];
                        columnRange.NumberFormat = "0";
                        columnRange.Value = columnRange.Value2;
                    }
                }
                // string modified_filePath = Path.Combine(outputFolder, Path.GetFileName(filePath));
                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Log("Error: " + ex.Message);
            }
            finally
            {
                workbook.Close(false);
                excelApp.Quit();

                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
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

                        int lastRow = worksheet.Cells[worksheet.Rows.Count, 1].End(Excel.XlDirection.xlUp).Row;


                        foreach (Excel.Range cell in columnRange.Cells)
                        {
                            if (cell.Value2 != null && cell.Value2.ToString() == "-0.00")
                            {
                                cell.Value2 = "0.00";
                            }
                            if(cell.Row > lastRow)
                            {
                                break;
                            }
                        }

                    }
                }
               
                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Log("Error: " + ex.Message);
            }
            finally
            {
                workbook.Close(false);
                excelApp.Quit();

                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        public static void AlignColumnData(string filePath) {

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
                Console.WriteLine("Error opening Excel file: " + ex.Message);
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
                            return;
                        }
                    }
                }

                workbook.SaveAs(csvFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Log("Error: " + ex.Message);
            }
            finally
            {
                workbook.Close(false);
                excelApp.Quit();
            }

        }

        public static void MergeCsvFilesToExcel(string filePath)
        {
            string arguments = args.ToString();
            string password= arguments.Split('|')[0];
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
                Console.WriteLine("Error opening Excel file: " + ex.Message);
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
                       // worksheet.Columns.AutoFit();

                        break;
                    }
                }
                //workbook.Worksheets[0].Activate();
                //workbook.Worksheets[0].Cells[1, 1].Select();
                //workbook.Worksheets[0].Cells[1, 1].Activate();
                workbook.SaveAs(csvFilePath);




            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Log("Error: " + ex.Message);
            }
            finally
            {
                workbook.Close(false);
                excelApp.Quit();
                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        public static Excel.Worksheet FindWorksheet(Excel.Workbook workbook, string worksheetName)
        {
            foreach (Excel.Worksheet worksheet in workbook.Worksheets)
            {
                if (worksheet.Name == worksheetName)
                {
                    return worksheet;
                }
            }
            return null;
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

                string csvFilePath = outputFolder + @"\"+fname + extension;

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
                        if(foundCells != null)
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
                Console.WriteLine("Error: " + ex.Message);
                Log("Error: " + ex.Message);
            }
            finally
            {
                workbook.Close(false);
                excelApp.Quit();
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
                }
                else
                {
                    Console.WriteLine("Worksheet '{0}' not found.", sheetNameToDelete);
                }
                workbook.SaveAs(csvFilePath);
            }
             catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Log("Error: " + ex.Message);
            }
            finally
            {
                // Close and release resources
                workbook.Close(false);
                excelApp.Quit();
                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
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
                Console.WriteLine("Error: " + ex.Message);
                Log("Error: " + ex.Message);
            }
            finally
            {
                // Close and release resources
                workbook.Close(false);
                excelApp.Quit();
                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }

        }


        public static void FindCellValueAndReplaceInColumn(string filePath)
        {
            string[] arr = args.Split('#');
            int columnIndex = int.Parse(arr[0]);
            string targetValue = arr[1];
            string replaceValue = arr[2];
            // Create an instance of Excel Application
            Application excelApp = new Application();
            excelApp.DisplayAlerts = false;

            // Open the Excel workbook
            Workbook workbook = excelApp.Workbooks.Open(filePath);

            // Get the worksheet
            Worksheet worksheet = (Worksheet)workbook.Sheets[1];

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
            workbook.Close();

            // Quit Excel application
            excelApp.Quit();

            // Release COM objects
            if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
            if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
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
                string csvFilePath = outputFolder+ @"\"+fname + ".csv";
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
                Log("Conversion to CSV completed for " + fname);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Log("Error: " + ex.Message);
            }
            finally
            {
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
                     Log("Error: The specified input file does not exist."); // Uncomment if you have a logging method
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                 Log("Error: " + ex.Message); // Uncomment if you have a logging method
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
                     Log($"File copied successfully to {csvFilePath}"); // Uncomment if you have a logging method
                }
                else
                {
                    Console.WriteLine("Error: The specified input file does not exist.");
                     Log("Error: The specified input file does not exist."); // Uncomment if you have a logging method
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                // Log("Error: " + ex.Message); // Uncomment if you have a logging method
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
                    // Log("Conversion completed. Text file saved at: " + outputFilePath); // Uncomment if you have a logging method
                }
                else
                {
                    Console.WriteLine("Error: The specified input file does not exist.");
                    // Log("Error: The specified input file does not exist."); // Uncomment if you have a logging method
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                // Log("Error: " + ex.Message); // Uncomment if you have a logging method
            }
        }

        static void ConvertCommaToColumns(string filePath)
        {
            Excel.Application excelApp = new Excel.Application();
            excelApp.DisplayAlerts = false;
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet worksheet = workbook.ActiveSheet;

            try
            {
               
                Excel.Range range = worksheet.UsedRange;
                range.TextToColumns(Destination: range, DataType: Excel.XlTextParsingType.xlDelimited,
                                     TextQualifier: Excel.XlTextQualifier.xlTextQualifierDoubleQuote,
                                     ConsecutiveDelimiter: false, Tab: false, Semicolon: false,
                                     Comma: true, Space: false, Other: false, OtherChar: ",", FieldInfo: null);

                Console.WriteLine("Text converted to columns separated by commas successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            finally
            {
                workbook.Save();
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
                csvFilePath = outputFolder+@"\" + fname + ".csv";
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
    true, // ConsecutiveDelimiter
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
                Log("Conversion text to column completed for " + fname);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Log("Error: " + ex.Message);
            }
            finally
            {
                // Close and release resources
                workbook.Close(false);
                excelApp.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
             ClearFormat(csvFilePath);
           // ClearFormatOfSheet(csvFilePath);


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
                string csvFilePath =outputFolder+@"\" + fname + ".xls";
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
                Log("Conversion to XLS completed for " + fname);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Log("Error: " + ex.Message);
            }
            finally
            {
                // Close and release resources
                workbook.Close(false);
                excelApp.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }

        }
        static void IterateFilesInFolder(string folderPath, Dictionary<string, List<Action<string>>> keywordFunctions,string outputFolder)
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
                            }
                            filecount++;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Directory does not exist.");
                Log("Directory does not exist.");
            }

            Console.WriteLine("Total Files Converted " + filecount);
            Log("Total Files Converted " + filecount);

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
                Console.WriteLine($"An error occurred: {ex.Message}");
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
            string[] matchingFiles= FindFilesWithPartialName(partialFilePath, partialFileName);
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

            // Create Excel Application instance
            Excel.Application excelApp = new Excel.Application();

            // Disable alerts and screen updating to improve performance
           // excelApp.DisplayAlerts = false;
            excelApp.ScreenUpdating = false;

            try
            {
                // Open main workbook
                Excel.Workbook mainWorkbook = excelApp.Workbooks.Open(mainFilePath);
               // otherFilePath = @"C:\\Nirvana\\Lyrical_Modification_Latest_0702\\ConsoleApplication1 - Copy\\ConsoleApplication1\\bin\\Debug\\Input_Files\\Triad 52\\52\\positions.csv";
                // Open other workbook
                Excel.Workbook otherWorkbook = excelApp.Workbooks.Open(otherFilePath);

                // Get main worksheet
                Excel.Worksheet mainSheet = mainWorkbook.Sheets[1]; // Assuming data is in the first worksheet

                // Get other worksheet
                Excel.Worksheet otherSheet = otherWorkbook.Sheets[1]; // Assuming data is in the first worksheet

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
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            finally
            {
                
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
                Console.WriteLine("An error occurred while releasing object: " + ex.Message);
            }
            finally
            {
                GC.Collect();
            }
        }


        public static void FilterAndDeleteRows(string filePath)
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
        }

       public static void FilterAndReplaceInOtherColumn(string filePath)
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

            // Release COM objects
            if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
            if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
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
                cellContent=cellContent.Trim();
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
            workbook.Close();
            excelApp.Quit();

            // Release COM objects
            if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            if (sheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
            if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
        }
        static bool SearchTextContainsItems(List<string> searchTextList, string searchText)
        {
            foreach (string item in searchTextList)
            {
                if (searchText.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }
        public static void FilterAndDeleteRowsListIfContainsStartFromRow(string filePath)
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

            Console.WriteLine("Rows not containing the specified text in the specified column have been deleted.");
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

            // Filter and delete rows
            Excel.Range usedRange = sheet.UsedRange;
            int rowCount = usedRange.Rows.Count;
            int startRowIndex = 1; // Assuming the first row is header

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
            workbook.Close();
            excelApp.Quit();

            // Release COM objects
            if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            if (sheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
            if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
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
            

            for (int rowIndex = startRowIndex; rowIndex <= rowCount; rowIndex++)
            {
                Excel.Range cell = (Excel.Range)sheet.Cells[rowIndex, columnIndex];
                string cellContent = cell.Value != null ? cell.Value.ToString() : "";
                if (cellContent.Equals(searchText) )
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
            workbook.Close();
            excelApp.Quit();

            // Release COM objects
            if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            if (sheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
            if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
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
            workbook.Close();
            excelApp.Quit();

            Console.WriteLine("Rows not containing the specified text in the specified column have been deleted.");
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
            workbook.Close();
            excelApp.Quit();

            // Release COM objects
            if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            if (sheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
            if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
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
        static void IterateAllFilesInFolder(string folderPath, Action<string> Function,string outputFolder)
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
                Log("Directory does not exist.");
            }

            Console.WriteLine("Total Files Converted " + filecount);
            Log("Total Files Converted " + filecount);

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

                }
            }
            catch (Exception ex)
            {
                logmessage += Environment.NewLine + $"Error clearing format of sheet in file '{filePath}': {ex.Message}";

            }

            Console.WriteLine(logmessage);

        }

        static void ClearFormat(string filePath)
        {
            string logmessage = "";
            Excel.Workbook workbook = null;
           Excel.Application excelApp = null;
            Excel.Worksheet worksheet = null;
            string fname = Path.GetFileNameWithoutExtension(filePath);
            string extension=Path.GetExtension(filePath);
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
                workbook.Close(false);
                excelApp.Quit();

                logmessage += Environment.NewLine + $"Cell styles cleared successfully in file: {filePath}";
                Log(logmessage);
            }
            catch (Exception ex)
            {
                logmessage += Environment.NewLine + $"Error clearing format of sheet in file '{filePath}': {ex.Message}";
                Log(logmessage);
            }
            finally
            {
                // Close and release resources
                //workbook.Close(false);
                //excelApp.Quit();

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

                logmessage += Environment.NewLine + $"Cell styles cleared successfully for columns in file: {filePath}";
            }
            catch (Exception ex)
            {
                logmessage += Environment.NewLine + $"Error clearing format of columns in file '{filePath}': {ex.Message}";
            }
            finally
            {
                if (workbook != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (excelApp != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }

            Console.WriteLine(logmessage);
            Log(logmessage);
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
                Log(logmessage);
            }
            catch (Exception ex)
            {
                logmessage += Environment.NewLine + $"Error clearing format of sheet in file '{filePath}': {ex.Message}";
                Log(logmessage);
            }
            finally
            {
               
            }

            Console.WriteLine(logmessage);
            Log(logmessage );

        }
    }
    
}
