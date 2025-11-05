using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;


namespace Prana.PM.MultiFormatParser
{
    [Formatting(DataSourceFileFormat.Csv)]
    class CsvReadingStrategy : FileFormatStrategy
    {
        public override DataTable GetDataTableFromUploadedDataFile(string fileName)
        {
            DataTable result = new DataTable();

            string inputReader = string.Empty;
            // check that the file exists before opening it
            if (File.Exists(fileName))
            {
                StreamReader fs = new StreamReader(fileName);
                //StreamReader sr = new StreamReader(path);
                inputReader = fs.ReadToEnd();
                fs.Close();

            }
            result = ParseCSV(inputReader);
            


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
        /// Parses the CSV.
        /// </summary>
        /// <param name="inputString">The input string.</param>
        /// <returns></returns>
        public DataTable ParseCSV(string inputString)
        {

            DataTable dt = new DataTable();

            // declare the Regular Expression that will match versus the input string
            Regex re = new Regex("((?<field>[^\",\\r\\n]+)|\"(?<field>([^\"]|\"\")+)\")(,|(?<rowbreak>\\r\\n|\\n|$))");

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

                if (field.Length > 0)
                {
                    colArray.Add(field);
                    colCount++;
                }

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
                dt.Columns.Add(String.Format("col{0:000}", i));

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

            // in case no data was parsed, create a single column
            if (dt.Columns.Count == 0)
                dt.Columns.Add("NoData");

            return dt;
        }

    }
}
