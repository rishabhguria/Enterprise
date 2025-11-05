using System;
using System.Data;
using System.IO;

namespace Prana.Utilities
{
    public class CSVFileHealper
    {

        public static void ProduceCSV(DataTable dt, DataSet dsSummary, string filePath, bool WriteHeader)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(filePath);
                DataTable dtsummary = null;
                if (dsSummary != null && dsSummary.Tables.Count > 0)
                {
                    dtsummary = dsSummary.Tables[0];
                }
                if (WriteHeader)
                {
                    string[] arr = new String[dt.Columns.Count];
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        arr[i] = GetWriteableValue(dt.Columns[i].ColumnName);
                    }

                    sw.WriteLine(string.Join(",", arr));
                }

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    string[] dataArr = new String[dt.Columns.Count];
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        object o = dt.Rows[j][i];
                        dataArr[i] = GetWriteableValue(o);
                    }
                    sw.WriteLine(string.Join(",", dataArr));
                }

                #region Summary
                if (dtsummary != null)
                {
                    string[] arrSummary = new String[dtsummary.Columns.Count];
                    for (int i = 0; i < dtsummary.Columns.Count; i++)
                    {
                        arrSummary[i] = GetWriteableValue(dtsummary.Columns[i].ColumnName);
                    }

                    sw.WriteLine(string.Join(",", arrSummary));



                    for (int j = 0; j < dtsummary.Rows.Count; j++)
                    {
                        string[] dataArr = new String[dtsummary.Columns.Count];
                        for (int i = 0; i < dtsummary.Columns.Count; i++)
                        {
                            object o = dtsummary.Rows[j][i];
                            dataArr[i] = GetWriteableValue(o);
                        }
                        sw.WriteLine(string.Join(",", dataArr));
                    }
                }
                #endregion


            }
            catch //(Exception ex)
            {
                throw;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();
                }
            }

        }
        public static string GetWriteableValue(object o)
        {
            if (o == null || o == Convert.DBNull)
                return "";
            else if (o.ToString().IndexOf(",") == -1)
                return o.ToString();
            else
                return "\"" + o.ToString() + "\"";

        }
    }
}
