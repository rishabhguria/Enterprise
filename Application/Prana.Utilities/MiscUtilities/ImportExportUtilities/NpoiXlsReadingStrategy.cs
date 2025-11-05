using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Prana.Utilities.ImportExportUtilities;
using System.Data;
using System.Globalization;
using System.IO;

namespace Prana.Utilities.MiscUtilities.ImportExportUtilities
{
    [Formatting(DataSourceFileFormat.Xls)]
    public class NpoiXlsReadingStrategy : FileFormatStrategy
    {
        public override DataTable GetDataTableFromUploadedDataFile(string fileName)
        {
            var sh = NpoiCommonHelper.GetFileStream(fileName, "XLS");
            var headerRow = sh.GetRow(0);
            int colCount = headerRow.LastCellNum;
            return NpoiCommonHelper.CreateDataTableByFileStream(sh, colCount);
        }
    }
}
