using System.Data;

namespace Prana.Utilities.ImportExportUtilities
{
    [Formatting(DataSourceFileFormat.Text)]
    public class TextReadingStrategy : FileFormatStrategy
    {
        public override DataTable GetDataTableFromUploadedDataFile(string fileName)
        {
            DataTable result = new DataTable();

            return result;
        }
    }
}
