using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using Prana.Global;
using System.Collections.Specialized;


namespace Prana.Utilities.ImportExportUtilities
{
    [Formatting(DataSourceFileFormat.Default)]
    class DefaultReadingStrategy : FileFormatStrategy
    {
        NameValueCollection regexExpressionColl = null;

        Dictionary<string, string> _regexExpressionDict = new Dictionary<string, string>();

        public override DataTable GetDataTableFromUploadedDataFile(string fileName)
        {
            DataTable result = new DataTable();
            regexExpressionColl = ConfigurationHelper.Instance.GetSectionBySectionName(ConfigurationHelper.SECTION_regexExpression);
          
            string inputReader = string.Empty;
            string filetype = string.Empty;
            // check that the file exists before opening it
            if (File.Exists(fileName))
            {
                filetype = fileName.Substring(fileName.LastIndexOf(".") + 1);
                StreamReader fs = new StreamReader(fileName);
                //StreamReader sr = new StreamReader(path);
                inputReader = fs.ReadToEnd();
                fs.Close();

            }
            result = ParseFile(inputReader, filetype, regexExpressionColl);
            


            //using (CachedCsvReader csv = new CachedCsvReader(new StreamReader(fs), false))
            //{
            //    //int fieldcount = csv.FieldCount;
            //    //string rawdata = csv.GetCurrentRawData();
            //    result.Load(csv);
            //   while(csv.ReadNextRecord())
            //   {
            //       //csv.CopyCurrentRecordTo
            //   }
                
            //}


            return result;
        }

        /// <summary>
        /// Parses any file.
        /// </summary>
        /// <param name="inputString">The input string.</param>
        /// <returns></returns>
        public DataTable ParseFile(string inputString, string fileType, NameValueCollection regexExpressionColl)
        {

            DataTable dt = new DataTable();
            string ParsingRegularExpressionOld = string.Empty;
            if (regexExpressionColl != null && regexExpressionColl.Count > 0)
            {
                if (regexExpressionColl[fileType.Trim().ToUpper()] != null)
                {
                    ParsingRegularExpressionOld = regexExpressionColl[fileType.Trim()];
                }
            }
           // string csvParsingRegularExpressionOld = Prana.Global.ConfigurationHelper.Instance.GetAppSettingValueByKey("CSVParsingRegularExpression");
            string parsingRegularExpression = ParsingRegularExpressionOld.Replace("\\\\", "\\");
            Regex re = new Regex(parsingRegularExpression);
            // declare the Regular Expression that will match versus the input string

            //string zeroOrMoreString = Prana.Global.ConfigurationHelper.Instance.GetAppSettingValueByKey("IsZeroOrMore");
            //bool isZeroOrMore = Convert.ToBoolean(zeroOrMoreString);
            ////Regex re = null;
            //if (isZeroOrMore)
            //{
            //    re = new Regex("((?<field>[^\",\\r\\n]*)|\"(?<field>([^\"]|\"\")*)\")(,|(?<rowbreak>\\r\\n|\\n|$))");
            //}
            //else
            //{
            //    re = new Regex("((?<field>[^\",\\r\\n]+)|\"(?<field>([^\"]|\"\")+)\")(,|(?<rowbreak>\\r\\n|\\n|$))");
            //}

            ArrayList colArray = new ArrayList();
            ArrayList rowArray = new ArrayList();

            int colCount = 0;
            int maxColCount = 0;
            string rowbreak = "";
            string field = "";

            MatchCollection mc = re.Matches(inputString);

            foreach (Match m in mc)
            {

                // retrieve the field and replace two double-quotes with a single double-quote
                field = m.Result("${field}").Replace("\"\"", "\"");

                rowbreak = m.Result("${rowbreak}");

                //if (field.Length > 0)
                //{
                //    colArray.Add(field);
                //    colCount++;
                //}
                colArray.Add(field);
                colCount++;

                if (rowbreak.Length > 0)
                {

                    // add the column array to the row Array List
                    rowArray.Add(colArray.ToArray());

                    // create a new Array List to hold the field values
                    colArray = new ArrayList();

                    if (colCount > maxColCount)
                        maxColCount = colCount;

                    colCount = 0;
                }
            }

            if (rowbreak.Length == 0)
            {
                // this is executed when the last line doesn't
                // end with a line break
                rowArray.Add(colArray.ToArray());
                if (colCount > maxColCount)
                    maxColCount = colCount;
            }

            // create the columns for the table
            for (int i = 0; i < maxColCount; i++)
                dt.Columns.Add("COL" + (i + 1).ToString(), typeof(string));

            // convert the row Array List into an Array object for easier access
            Array ra = rowArray.ToArray();
            for (int i = 0; i < ra.Length; i++)
            {

                // create a new DataRow
                DataRow dr = dt.NewRow();

                // convert the column Array List into an Array object for easier access
                Array ca = (Array)(ra.GetValue(i));

                // add each field into the new DataRow
                for (int j = 0; j < ca.Length; j++)
                    dr[j] = ca.GetValue(j);

                // add the new DataRow to the DataTable
                dt.Rows.Add(dr);
            }

            if(dt.Rows.Count>0)
            RemoveEmptyRows(dt);

            // in case no data was parsed, create a single column
            if (dt.Columns.Count == 0)
                dt.Columns.Add("NoData");

            return dt;
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
