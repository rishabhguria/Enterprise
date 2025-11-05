using System;
using System.Collections.Generic;
using System.Text;
//using Nirvana.PM.BLL;
using System.Data;
//using Nirvana.PM.MultiLayoutParse.Components;
using Nirvana.BusinessObjects.PositionManagement;

namespace Nirvana.PM.MultiLayoutParse
{
    public abstract class FileParsingStrategy
    {

        public abstract int ParseFileAndStoreData(RunUpload currentFtpProgress);

        public abstract bool ParseFileAndStoreData(RunUpload currentFtpProgress, DataTable datasourceData);

        /// <summary>
        /// Validates the header row the file recieved.
        /// </summary>
        /// <param name="headerRow">The header row.</param>
        /// <param name="columnList">The column list.</param>
        /// <returns></returns>
        protected bool ValidateFileHeaderRow(DataRow headerRow, DataSourceColumnList columnList, int columnCountINFile)
        {
            bool isValid = true;
            try
            {

                // subtracting 1 from the columncount for iteration, because the last column has been added by us, and will not 
                // found there in nirvana.
                for (int counter = 0; counter < columnList.Count - 1; counter++)
                {
                    try
                    {
                        bool isRequiredColumn = columnList[counter].IsRequiredInUpload;
                        string fileColumnHeaderName = string.Empty;
                        if (counter <= columnCountINFile - 1)
                        {
                            fileColumnHeaderName = headerRow[counter].ToString().Replace(" ", "").Replace("/", "_");
                        }
                        string setColumnHeaderName = columnList[counter].ColumnName;
                        if (isValid)
                        {
                            if (!isRequiredColumn)
                            {
                                continue;
                            }
                            //TODO: uncomment it. commented for now as to xls reader having problems is reading more than 16 columns.
                            if (isRequiredColumn && !string.Equals(fileColumnHeaderName.ToUpperInvariant(), setColumnHeaderName.ToUpperInvariant()))
                            {
                                isValid = false;
                                return isValid;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //string exceptionMessage = ex.Message;
                        isValid = false;
                    }

                }
            }
            catch (Exception ex)
            {
                //string exceptionMessage = ex.Message;
                isValid = false;
            }


            return isValid;

        }

    }

}
