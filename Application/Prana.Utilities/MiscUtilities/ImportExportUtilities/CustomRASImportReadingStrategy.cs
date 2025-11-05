using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Prana.LogManager;
using System;
using System.Data;
using System.Globalization;
using System.IO;

namespace Prana.Utilities.MiscUtilities.ImportExportUtilities
{
    public class CustomRASImportReadingStrategy
    {
        /// <summary>
        /// This method is created to handle the reading of xls and xlsx files from Import module of Rebalancer.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static DataTable GetDataTableFromUploadedDataFile(string fileName, string fileFormat)
        {
            DataTable dtExcelTable = null;
            try
            {
                var sh = NpoiCommonHelper.GetFileStream(fileName, fileFormat);
                int colCount = 0;
                var i = 0;
                var currentRow = sh.GetRow(i);
                while (currentRow != null)
                {
                    if (colCount < currentRow.Cells.Count)
                    {
                        colCount = currentRow.Cells.Count;
                    }
                    i++;
                    currentRow = sh.GetRow(i);
                }
                dtExcelTable = NpoiCommonHelper.CreateDataTableByFileStream(sh, colCount);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dtExcelTable;
        }
    }
}
