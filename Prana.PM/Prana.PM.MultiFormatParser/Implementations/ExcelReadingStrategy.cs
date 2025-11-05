using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;


namespace Prana.PM.MultiFormatParser
{
    [Formatting(DataSourceFileFormat.Excel)]
    class ExcelReadingStrategy : FileFormatStrategy 
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
                result = spreadSheet.WorkbookData.Tables[0];                
            }
            catch (Exception ex)
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
