using Nirvana.TestAutomation.Interfaces;
using System;
using System.Data;
using System.IO;
using OfficeOpenXml;
using System.Linq;
using TestAutomationFX.Core;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ExcelDataReader;
using System.Diagnostics.CodeAnalysis;

namespace Nirvana.TestAutomation.TestDataProvider
{
    public class XlsDataProvider : ITestDataProvider
    {
        /// <summary>
        /// Gets the test data.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="rowHeaderIndex">Index of the row header.</param>
        /// <param name="startColumnFrom">The start column from.</param>
        /// <returns></returns>
        public DataSet GetTestData(string filePath, int rowHeaderIndex = 1, int startColumnFrom = 1, string fileType = "")
        {
            return LoadXlsFile(filePath, rowHeaderIndex, startColumnFrom);
        }

       

        /// <summary>
        /// Loads the excel file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="headerIndex">Index of the header.</param>
        /// <param name="columnFrom">The column from.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        private DataSet LoadXlsFile(string filePath, int headerIndex, int columnFrom)
        {
            DataSet result = new DataSet();
            try
            {
                 using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                // Auto-detect format, supports:
                //  - Binary Excel files (2.0-2003 format; *.xls)
                //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
                using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                {
                    // 2. Use the AsDataSet extension method
                    result = reader.AsDataSet();
                    if (result != null)
                    {
                        for (int i = 0; i < result.Tables.Count; i++)
                        {
                            DataTable dt = result.Tables[i];
                            if (dt.Rows.Count > 0)
                            {
                                int index = 0;
                                foreach (DataColumn col in dt.Columns)
                                {
                                    col.ColumnName = dt.Rows[0][index].ToString();
                                    index++;
                                }
                                dt.Rows.RemoveAt(0);
                            }
                        }
                    }
                    // The result of each spreadsheet is in result.Tables
                }
            }
                
        }
    
            
            catch (Exception ex)
            {
                Log.Information("Error: " + ex.Message);
            }
            return result;
        }
    }
}
