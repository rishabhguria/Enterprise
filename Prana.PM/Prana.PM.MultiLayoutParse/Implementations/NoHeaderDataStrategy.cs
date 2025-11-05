using System;
using System.Collections.Generic;
using System.Text;
using Prana.PM.BLL;
using Prana.PM.DAL;
using System.Data;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Prana.PM.MultiLayoutParse
{
    [ParsingAttribute(DataSourceFileLayout.NoHeaderData)]
    public class NoHeaderDataStrategy : FileParsingStrategy
    {
        public override int ParseFileAndStoreData(RunUpload currentFtpProgress)
        {
            return Convert.ToInt16(DataSourceFileLayout.NoHeaderData);
        }

        /// <summary>
        /// Parses the file and store data.
        /// </summary>
        /// <param name="ParentRunUpload">The parent run upload.</param>
        /// <param name="fileData">The file data.</param>
        /// <returns></returns>
        public override bool ParseFileAndStoreData(RunUpload ParentRunUpload, DataTable fileData)
        {
            try
            {
                bool isSuccessfull = false;
                //int totalRecords = 0;
                DataRowCollection rows = fileData.Rows;                
                DataColumnCollection columns = fileData.Columns;

                if (int.Equals(rows.Count, 0) || int.Equals(columns.Count, 0))
                {
                    ParentRunUpload.StatusDescription = "Empty File Recieved";
                    return isSuccessfull;
                }

                DataSourceColumnList columnList = RunUploadManager.PMGetDataSourceColumnsByID(ParentRunUpload.DataSourceNameIDValue.ID, ParentRunUpload.TableTypeID);

                if (columnList.Count > 0 && columns.Count > 0)
                {
                    int rowcount = 0;
                    foreach (DataRow row in rows)
                    {
                        rowcount++;
                        // subtract 1 from the count because last column is the UploadID column added by us, which will
                        // never be found in the datasourcefile.
                        for (int columnIndex = 0; columnIndex < columnList.Count - 1; columnIndex++)
                        {
                            if (columnList[columnIndex].IsRequiredInUpload)
                            {
                                if (row[columnIndex] == null || int.Equals(Convert.ToString(row[columnIndex]).Trim().Length, 0))
                                {  
                                    ParentRunUpload.StatusDescription = columnList[columnIndex].ColumnName + " is required field. Data in one or more row for this field is missing in the file";
                                    return isSuccessfull;                                    
                                }
                                else
                                {
                                    string cellvalue = Convert.ToString(row[columnIndex]);
                                    string cellType = columnList[columnIndex].Type;
                                    bool isValidValue = false;
                                    switch (columnList[columnIndex].Type)
                                    {
                                            case "Integer":
                                                int i;
                                                isValidValue = int.TryParse(cellvalue, out i);
                                            break;
                                            case "Decimal":
                                                decimal dec;
                                                isValidValue = decimal.TryParse(cellvalue, out dec);
                                            break;
                                            case "Boolean":
                                                bool booleanval;
                                                isValidValue = bool.TryParse(cellvalue, out booleanval);
                                            break;
                                            case "DateTime":
                                                DateTime dateTime;
                                                isValidValue = DateTime.TryParse(cellvalue, out dateTime);
                                            break;
                                            case "Text":
                                                isValidValue = true;
                                                break;
                                        default:
                                            break;
                                    }
                                    if (!isValidValue)
                                    {
                                        ParentRunUpload.StatusDescription = columnList[columnIndex].ColumnName + " is a " + cellType + " type . Data in one or more row for this field cannot be converted into this format.";
                                        return isSuccessfull;    
                                    }

                                    
                                }                                    
                            }
                        }
                    }
                }
                else
                {
                    ParentRunUpload.StatusDescription = "No Columns are set up for this Data Source.";
                    return isSuccessfull;
                }

                

                ParentRunUpload.Status = RunUploadStatus.Successful;
                ParentRunUpload.StatusDescription = "Successful";
                ParentRunUpload.LastRunUploadDate = DateTime.UtcNow;
                ParentRunUpload.TotalRecords = rows.Count;
                //ParentRunUpload.FirstRecordIndex = firstRecordIndex;
                //ParentRunUpload.HeaderIndex = headerIndex;

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


            //return false;
            //return Convert.ToInt16(DataSourceFileLayout.NoHeaderData);
        }
    }
}
