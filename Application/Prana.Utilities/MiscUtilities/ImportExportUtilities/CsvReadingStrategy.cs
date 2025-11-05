using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;


namespace Prana.Utilities.ImportExportUtilities
{
    [Formatting(DataSourceFileFormat.Csv)]
    public class CsvReadingStrategy : FileFormatStrategy
    {
        public override DataTable GetDataTableFromUploadedDataFile(string fileName)
        {
            //http://stackoverflow.com/questions/8307557/datatable-inside-using
            DataTable dtCSVToDataTable = new DataTable();
            // extract the fields based on regular expression               
            Regex regex = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            //read all lines from csv file
            string[] lines = File.ReadAllLines(fileName);
            //send empty data table if the there are no lines in the file
            if (lines.Length <= 0)
            {
                return dtCSVToDataTable;
            }
            //columns will be picked from the first line of the csv file
            string[] Columns = (lines[0].Split(','));
            //add columns to the datatable
            //no of columns will be added same as of items in the first line of csv file
            for (int i = 0; i < Columns.Length; i++)
            {
                dtCSVToDataTable.Columns.Add("COL" + (i + 1).ToString(), typeof(string));
            }
            //add rows to the datatable
            for (int i = 0; i < lines.Length; i++)
            {
                //read each line and add to the DataTable
                string[] items = regex.Split(lines[i]);
                DataRow row = dtCSVToDataTable.NewRow();
                for (int j = 0; j < items.Length; j++)
                {
                    //adjust/add no. of columns on the basis of fields in the data row
                    //it may be possible in production files that no of columns to the csv file will not remain same as of first line, may be increaesd
                    //first line may contain date/time, client name or file details
                    if (j > (dtCSVToDataTable.Columns.Count - 1))
                    {
                        dtCSVToDataTable.Columns.Add("COL" + (j + 1).ToString(), typeof(string));
                    }
                    items[j] = items[j].TrimStart(' ', '"').TrimEnd('"');
                    row[j] = items[j].Replace("\"\"", "\"");
                }
                dtCSVToDataTable.Rows.Add(row);
            }
            if (dtCSVToDataTable.Rows.Count > 0)
                RemoveEmptyRows(dtCSVToDataTable);
            // in case no data was parsed, create a single column
            if (dtCSVToDataTable.Columns.Count == 0)
                dtCSVToDataTable.Columns.Add("NoData");

            return dtCSVToDataTable;
        }

        private void RemoveEmptyRows(DataTable dt)
        {
            List<DataRow> emptyRows = new List<DataRow>();
            foreach (DataRow row in dt.Rows)
            {
                bool allrowEmpty = true;
                foreach (object item in row.ItemArray)
                {
                    if (item.ToString() != string.Empty)
                    {
                        allrowEmpty = false;
                    }
                }

                if (allrowEmpty)
                {
                    emptyRows.Add(row);
                }
            }

            foreach (DataRow row in emptyRows)
            {
                dt.Rows.Remove(row);
            }
        }
    }
}
