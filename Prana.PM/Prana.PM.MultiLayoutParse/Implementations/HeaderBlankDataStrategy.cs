using System;
using System.Collections.Generic;
using System.Text;
using Prana.PM.BLL;
using Prana.PM.DAL;
//using Prana.PM.MultiLayoutParse.Components;
using Prana.BusinessObjects.PositionManagement;
using Prana.BusinessObjects.AppConstants;
using System.Data;

namespace Prana.PM.MultiLayoutParse
{
    [ParsingAttribute(DataSourceFileLayout.HeaderBlankData)]
    class HeaderBlankDataStrategy : FileParsingStrategy
    {

        public override int ParseFileAndStoreData(RunUpload currentFtpProgress)
        {
            return Convert.ToInt16(DataSourceFileLayout.HeaderBlankData);
        }

        public override bool ParseFileAndStoreData(RunUpload ParentRunUpload, DataTable datasourceData)
        {
            
            int headerIndex = 0;
            int totalRecords = 0;

            DataRowCollection rows = datasourceData.Rows;
            DataColumnCollection columns = datasourceData.Columns;

            DataSourceColumnList columnList = RunUploadManager.PMGetDataSourceColumnsByID(ParentRunUpload.DataSourceNameIDValue.ID, ParentRunUpload.TableTypeID);

            bool hasValidHeader = true;
            foreach (DataRow row in rows)
            {
                ++headerIndex;

                /// TODO : Need to check here if the columns are in sequence as saved in the db, else through error
                /// "Columns are out of sequence".

                if (columns.Count > 0 && columnList.Count > 0)
                {
                    if (string.Equals(row[0].ToString(), columnList[0].ColumnName))
                    {
                        hasValidHeader = ValidateFileHeaderRow(row, columnList, columns.Count);
                        break;
                    }
                }
            }

            if (!hasValidHeader)
            {
                ParentRunUpload.Status = RunUploadStatus.Corrupted;
                ParentRunUpload.Progress = 0;
                ParentRunUpload.StatusDescription = "Corrupt file recieved. The Header of the file is as per the set up.";
                return false;
                
            }


            if (headerIndex == rows.Count)
            {
                ParentRunUpload.Status = RunUploadStatus.Corrupted;
                ParentRunUpload.Progress = 0;
                ParentRunUpload.StatusDescription = "Corrupted file recieved. Could not find header. File Discarded.";
                return false;
            }

            bool isFirstTime = true;
            int firstRecordIndex = 0;
            for (int rowIndex = headerIndex; rowIndex < rows.Count; rowIndex++)
            {
                ///Check for all of the compulsory columns, If any of the column's data is missing, Completely ignore 
                ///the file import process
                if (!(rows[rowIndex][0] is System.DBNull))
                {
                    if (isFirstTime)
                    {
                        ///Added one to avoid the zero based indexing
                        firstRecordIndex = rowIndex + 1;
                        isFirstTime = false;
                    }
                    ++totalRecords;
                    ///This function is only called to take the overview of the file, not to fetch the data,
                    ///hence column loop is not used
                    //for (int colIndex = 0; colIndex < columns.Count; colIndex++)
                    //{

                    //}
                }
            }

            ParentRunUpload.Status = RunUploadStatus.Successful;
            ParentRunUpload.StatusDescription = "Successful";
            ParentRunUpload.TotalRecords = totalRecords;
            ParentRunUpload.FirstRecordIndex = firstRecordIndex;
            ParentRunUpload.HeaderIndex = headerIndex;

            // _successfullUploads.Clear();

            // if(ParentRunUpload.StatusDescription == "Successful")
            // {
            //    _successfullUploads.Add(ParentRunUpload);
            // }
            //if (_successfullUploads.Count > 0)
            // {
            // ParentRunUpload.UploadID = IDGenerator();

            ParentRunUpload.UploadID = RunUploadManager.SaveUpLoadRunData(ParentRunUpload);
            return true;
        }

        
    }
}
