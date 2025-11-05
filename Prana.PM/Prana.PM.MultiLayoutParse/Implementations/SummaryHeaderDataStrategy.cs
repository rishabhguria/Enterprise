using System;
using System.Collections.Generic;
using System.Text;
using Prana.PM.BLL;
using Prana.PM.DAL;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Prana.BusinessObjects.PositionManagement;
using Prana.BusinessObjects.AppConstants;
//using Prana.PM.MultiLayoutParse.Components;


namespace Prana.PM.MultiLayoutParse
{
    [ParsingAttribute(DataSourceFileLayout.SummaryHeaderData)]
    public class SummaryHeaderDataStrategy : FileParsingStrategy
    {
        /// <summary>
        /// Parses the file and store data.
        /// </summary>
        /// <param name="runUploadItem">The run upload item.</param>
        /// <returns></returns>
        public override int ParseFileAndStoreData(RunUpload parentRunUpload)
        {
            return Convert.ToInt16(DataSourceFileLayout.SummaryHeaderData);
        }

        /// <summary>
        /// Parses the file and store data.
        /// </summary>
        /// <param name="runUploadItem">The run upload item.</param>
        /// <returns></returns>
        public override bool ParseFileAndStoreData(RunUpload ParentRunUpload, DataTable datasourceData)
        {

            try
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
                        if (string.Equals(row[0].ToString().ToUpperInvariant(), columnList[0].ColumnName.ToUpperInvariant()))
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
                    ParentRunUpload.StatusDescription = "Corrupt file recieved. The Header of the file is not per set up.";
                    return false;

                }


                if (headerIndex == rows.Count)
                {
                    ParentRunUpload.Status = RunUploadStatus.Corrupted;
                    ParentRunUpload.Progress = 0;
                    ParentRunUpload.StatusDescription = "Corrupted file recieved. File Discarded.";
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
                ParentRunUpload.LastRunUploadDate = DateTime.UtcNow;
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
                
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.Common.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return true;
            
        }
    }
}
