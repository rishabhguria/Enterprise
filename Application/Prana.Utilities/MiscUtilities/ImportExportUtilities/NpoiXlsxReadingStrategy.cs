using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Prana.Utilities.ImportExportUtilities;
using System.Data;
using System.Globalization;
using System.IO;

namespace Prana.Utilities.MiscUtilities.ImportExportUtilities
{
    [Formatting(DataSourceFileFormat.Xlsx)]
    public class NpoiXlsxReadingStrategy : FileFormatStrategy
    {
        public override DataTable GetDataTableFromUploadedDataFile(string fileName)
        {
            var sh = NpoiCommonHelper.GetFileStream(fileName, "XLSX");
            var headerRow = sh.GetRow(0);
            int colCount = headerRow.LastCellNum;
            return NpoiCommonHelper.CreateDataTableByFileStream(sh, colCount);
        }
    }
}
