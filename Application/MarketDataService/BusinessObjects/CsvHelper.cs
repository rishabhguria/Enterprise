using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Linq;
using System.Net;


namespace BusinessObjects
{
    public static class CsvHelper
    {
        public static DataTable GetDataTableFromUploadedDataFile(string fileName)
        {
            //http://stackoverflow.com/questions/8307557/datatable-inside-using
            using (DataTable dtCSVToDataTable = new DataTable())
            {
                try
                {
                    //string csvParsingRegularExpressionOld = Prana.Global.ConfigurationHelper.Instance.GetAppSettingValueByKey("CSVParsingRegularExpression");            //string csvParsingRegularExpression = csvParsingRegularExpressionOld.Replace("\\\\", "\\");
                    //// extract the fields based on regular expression   
                    //Regex regex = new Regex(csvParsingRegularExpression);
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
                            items[j] = items[j].TrimStart(' ', '"');
                            items[j] = items[j].TrimEnd('"');
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
                catch (Exception)
                {
                    return dtCSVToDataTable;
                }
            }
        }


        /// <summary>
        /// Copy of GetDataTableFromUploadedDataFile function...
        /// Added additional feture - Table name = file name, first line to be treated as header
        /// TODO : Need to refactor the these 2 methods to reutilize the logic
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static DataTable GetDataTableFromCSVWithHeader(string fileName)
        {
            //http://stackoverflow.com/questions/8307557/datatable-inside-using
            using (DataTable dtCSVToDataTable = new DataTable(Path.GetFileNameWithoutExtension(fileName)))
            {
                try
                {
                    //string csvParsingRegularExpressionOld = Prana.Global.ConfigurationHelper.Instance.GetAppSettingValueByKey("CSVParsingRegularExpression");            //string csvParsingRegularExpression = csvParsingRegularExpressionOld.Replace("\\\\", "\\");
                    //// extract the fields based on regular expression   
                    //Regex regex = new Regex(csvParsingRegularExpression);
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
                        // Added first line as header
                        dtCSVToDataTable.Columns.Add(Columns[i], typeof(string));
                    }
                    //add rows to the datatable
                    for (int i = 1; i < lines.Length; i++)
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
                            items[j] = items[j].TrimStart(' ', '"');
                            items[j] = items[j].TrimEnd('"');
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
                catch (Exception)
                {
                    return dtCSVToDataTable;
                }
            }

        }

        /// <summary>
        /// Datas the table to CSV.
        /// </summary>
        /// <param name="dtDataTable">The dt data table.</param>
        /// <param name="strFilePath">The string file path.</param>
        public static void DataTableToCSV(DataTable dtDataTable, string strFilePath)
        {
            try
            {
                StreamWriter sw = new StreamWriter(strFilePath, false);
                //headers  
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    sw.Write(dtDataTable.Columns[i]);
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
                foreach (DataRow dr in dtDataTable.Rows)
                {
                    for (int i = 0; i < dtDataTable.Columns.Count; i++)
                    {
                        if (!Convert.IsDBNull(dr[i]))
                        {
                            string value = dr[i].ToString();
                            if (value.Contains(','))
                            {
                                value = String.Format("\"{0}\"", value);
                                sw.Write(value);
                            }
                            else
                            {
                                sw.Write(dr[i].ToString());
                            }
                        }
                        if (i < dtDataTable.Columns.Count - 1)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.Write(sw.NewLine);
                }
                sw.Close();
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Removes the empty rows.
        /// </summary>
        /// <param name="dt">The dt.</param>
        private static void RemoveEmptyRows(DataTable dt)
        {
            try
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
            catch (Exception)
            {
            }
        }
    }
}
