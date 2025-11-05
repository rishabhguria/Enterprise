using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nirvana.AssemblyAndClassNameWriter
{
    public class AssemblyAndClassNameWriter
    {
        static string ApplicationStartUpPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        static string StepMappingFilePath = ApplicationStartUpPath + "\\TestAutomation.Steps";
        public static void Main()
        {
            try
            {
                System.Data.DataTable dataTable = new System.Data.DataTable(ExcelStructureConstants.COL_MODULES);
                dataTable.Columns.Add(ExcelStructureConstants.COL_MODULES);
                dataTable.Columns.Add(ExcelStructureConstants.COL_STEPS);

                if (Directory.Exists(StepMappingFilePath))
                {
                    foreach (string assemblyName in Directory.GetFiles(StepMappingFilePath, "*.dll"))
                    {
                        Assembly assembly = Assembly.LoadFrom(assemblyName);

                        if (assembly != null)
                        {
                            foreach (Type type in assembly.GetTypes())
                            {
                                if (type != null)
                                {
                                    var methodInfo = type.GetMethod("RunTest");
                                    if (methodInfo != null && Activator.CreateInstance(type) is ITestStep)
                                    {
                                        dataTable.Rows.Add(assembly.GetName().Name.Replace("Nirvana.TestAutomation.Steps.", ""), type.Name);
                                    }
                                }
                            }
                        }
                    }

                    ExportDataSetToExcel(dataTable);
                }
                else
                {
                    Console.WriteLine("Specified path not exists.");
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.StackTrace);
            }
            Console.Write("Press any key for exit...");
            Console.ReadKey();
        }

        private static void ExportDataSetToExcel(System.Data.DataTable dataTable)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelApp.Visible = false;
                excelApp.DisplayAlerts = false;

                Microsoft.Office.Interop.Excel.Workbook excelWorkBook = excelApp.Workbooks.Add(Type.Missing);
                Microsoft.Office.Interop.Excel.Worksheet excelWorkSheet = excelWorkBook.Sheets.Add();
                excelWorkSheet.Name = dataTable.TableName;

                for (int i = 1; i < dataTable.Columns.Count + 1; i++)
                {
                    excelWorkSheet.Cells[1, i] = dataTable.Columns[i - 1].ColumnName;
                }

                for (int j = 0; j < dataTable.Rows.Count; j++)
                {
                    for (int k = 0; k < dataTable.Columns.Count; k++)
                    {
                        excelWorkSheet.Cells[j + 2, k + 1] = dataTable.Rows[j].ItemArray[k].ToString();
                    }
                }

                Directory.CreateDirectory(StepMappingFilePath);
                excelWorkBook.SaveAs(StepMappingFilePath + ExcelStructureConstants.FILE_STEPS_MAPPING);
                excelWorkBook.Close();
                excelApp.Quit();
                Console.WriteLine("File Has been generated successfully at \"" + StepMappingFilePath + ExcelStructureConstants.FILE_STEPS_MAPPING + "\"");
            }
            catch
            {
                throw;
            }
        }
    }
}