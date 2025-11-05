using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Nirvana.TestAutomation.Utilities
{
    public static class CSVHelper
    {
        public static DataTable CSVAsDataTable(String csv)
        {
            DataTable dt = new DataTable("Data");
            try
            {
                String[] lines = csv.TrimEnd('\n').Split('\n');
                String line;
                line = lines[0];
                foreach (String column in line.TrimEnd("#~#", StringComparison.InvariantCulture).Split(new[] { "#~#" }, StringSplitOptions.None))
                    dt.Columns.Add(column);
                for (int i = 1; i < lines.Length; i++)
                    dt.Rows.Add(lines[i].TrimEnd("#~#", StringComparison.InvariantCulture).Split(new[] { "#~#" }, StringSplitOptions.None));
               
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return dt;
        }



        /// <summary>
        /// Read and Convert .txt file into DataTable
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static DataTable GetDataSourceFromFile(string fileName)
        {
            DataTable dTable = new DataTable();
            try
            {
                string[] columns = null;

                var lines = File.ReadAllLines(fileName);

                // assuming the first row contains the columns information
                if (lines.Count() > 0)
                {
                    columns = lines[0].Split(new char[] { ',' });

                    foreach (var column in columns)
                        dTable.Columns.Add(column);
                }

                // reading rest of the data
                for (int i = 1; i < lines.Count(); i++)
                {
                    DataRow dr = dTable.NewRow();
                    string[] values = lines[i].Split(new char[] { ',' });

                    for (int j = 0; j < values.Count() && j < columns.Count(); j++)
                        dr[j] = values[j];

                    dTable.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return dTable;
        }
    }
}
