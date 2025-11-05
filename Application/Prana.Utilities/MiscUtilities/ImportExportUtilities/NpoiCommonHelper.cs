using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.Utilities.MiscUtilities.ImportExportUtilities
{
    static class NpoiCommonHelper
    {
        /// <summary>
        /// This method is for creating datatable from file stream
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="columnCount"></param>
        /// <returns></returns>
        public static DataTable CreateDataTableByFileStream(ISheet sheet, int columnCount)
        {
            if (sheet == null) return null;

            var dataTable = new DataTable();
            for (var c = 0; c < columnCount; c++)
            {
                dataTable.Columns.Add("COL" + (c + 1).ToString());
            }

            try
            {
                var i = 0;
                var currentRow = sheet.GetRow(i);
                while (currentRow != null)
                {
                    var dr = dataTable.NewRow();
                    for (var j = 0; j < columnCount; j++)
                    {
                        var cell = currentRow.GetCell(j);

                        if (cell != null)
                            switch (cell.CellType)
                            {
                                case CellType.Numeric:
                                    dr[j] = DateUtil.IsCellDateFormatted(cell)
                                        ? cell.DateCellValue.ToString(CultureInfo.InvariantCulture)
                                        : cell.NumericCellValue.ToString(CultureInfo.InvariantCulture);
                                    break;
                                case CellType.String:
                                    dr[j] = cell.StringCellValue;
                                    break;
                                case CellType.Blank:
                                    dr[j] = string.Empty;
                                    break;
                            }
                    }
                    dataTable.Rows.Add(dr);
                    i++;
                    currentRow = sheet.GetRow(i);
                }
            }catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            return dataTable;
        }

        /// <summary>
        /// This method is for getting filestream for Xls and Xlsx files.
        /// </summary>
        /// <param name="fullFilePath"></param>
        /// <param name="fileFormat"></param>
        /// <returns></returns>
        public static ISheet GetFileStream(string fullFilePath, string fileFormat)
        {
            if(string.IsNullOrEmpty(fullFilePath) || string.IsNullOrEmpty(fileFormat))
            {
                return null;
            }
            using (var fs = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read))
            {
                return fileFormat.ToUpperInvariant().Equals("XLS") ? new HSSFWorkbook(fs).GetSheetAt(0) : new XSSFWorkbook(fs).GetSheetAt(0);
            }
        }
    }
}
