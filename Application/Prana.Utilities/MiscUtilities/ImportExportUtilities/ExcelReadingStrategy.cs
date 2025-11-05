using Prana.Utilities.ImportExportUtilities.Excel;
using System;
using System.Data;
using System.IO;

namespace Prana.Utilities.ImportExportUtilities
{
    [Formatting(DataSourceFileFormat.Excel)]
    public class ExcelReadingStrategy : FileFormatStrategy
    {

        public override DataTable GetDataTableFromUploadedDataFile(string fileName)
        {
            DataTable result = new DataTable();

            ExcelDataReader spreadSheet = null;
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);

            try
            {
                spreadSheet = new ExcelDataReader(fs);

                //int headerIndex = 0;
                //int totalRecords = 0;

                if (spreadSheet == null && spreadSheet.WorkbookData == null && spreadSheet.WorkbookData.Tables == null && spreadSheet.WorkbookData.Tables.Count == 0)
                {
                    //return null;
                    //currentFtpProgress.ParentRunUpload.Status = RunUploadStatus.Corrupted;
                    //currentFtpProgress.ParentRunUpload.StatusDescription = "Corrupted file recived. Please reimport the file.";

                    return result;
                }

                ///Find the row where data starts
                if (spreadSheet.WorkbookData.Tables.Count > 0)
                    result = spreadSheet.WorkbookData.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }


            return result;
        }
    }
}
