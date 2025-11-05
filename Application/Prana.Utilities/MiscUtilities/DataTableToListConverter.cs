using Prana.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Prana.Utilities.MiscUtilities
{
    //Generic Alternate - http://social.msdn.microsoft.com/Forums/en-US/netfxbcl/thread/29accbbd-5640-477c-b367-44f888bafde3/?prof=required&ppud=4
    public class DataTableToListConverter
    {
        public static List<string> GetListFromDataTable(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }
            List<string> list = new List<string>();

            StringBuilder sb = new StringBuilder();
            sb.Append("Header");
            sb.Append(Seperators.SEPERATOR_5);
            foreach (DataColumn dc in dt.Columns)
            {
                sb.Append(dc.ColumnName.ToString());
                sb.Append(Seperators.SEPERATOR_12);
                sb.Append(dc.DataType.ToString());
                sb.Append(Seperators.SEPERATOR_5);
            }

            // Header row added
            list.Add(sb.ToString().TrimEnd(Seperators.SEPERATOR_5));
            foreach (DataRow dr in dt.Rows)
            {
                sb = new StringBuilder();

                foreach (DataColumn dc in dt.Columns)
                {
                    sb.Append(dc.ColumnName);
                    sb.Append(Seperators.SEPERATOR_12);
                    sb.Append(dr[dc].ToString());
                    sb.Append(Seperators.SEPERATOR_5);
                }

                list.Add(sb.ToString().TrimEnd(Seperators.SEPERATOR_5));
            }

            return list;
        }



        public static DataTable GetDataTableFromList(List<string> list)
        {
            if (list == null)
            {
                return null;
            }

            DataTable dt = new DataTable();
            int headerIndex = 0;
            foreach (string str in list)
            {
                if (str.Contains("Header"))
                {
                    headerIndex = list.IndexOf(str);
                    string[] headerStr = str.Split(new char[] { Seperators.SEPERATOR_5 });

                    foreach (string col in headerStr)
                    {
                        string[] colNameDataType = col.Split(new char[] { Seperators.SEPERATOR_12 });
                        if (colNameDataType.Length > 1)
                        {
                            DataColumn dc = new DataColumn(colNameDataType[0], Type.GetType(colNameDataType[1]));
                            dt.Columns.Add(dc);
                        }
                    }
                    break;
                }
                else
                {
                    continue;
                }
            }
            list.RemoveAt(headerIndex);

            ///Parse data and put into datatable
            foreach (string dataStr in list)
            {
                DataRow dr = dt.NewRow();
                string[] fields = dataStr.Split(new char[] { Seperators.SEPERATOR_5 });
                foreach (string keyValue in fields)
                {
                    string[] keyValueArr = keyValue.Split(Seperators.SEPERATOR_12);
                    if (keyValueArr.Length > 1)
                    {
                        string columnName = keyValueArr[0];
                        DataColumn dc = dt.Columns[columnName];
                        switch (dc.DataType.ToString())
                        {
                            case "System.String":
                            default:
                                dr[dc] = keyValueArr[1].ToString();
                                break;

                            case "System.Double":
                                dr[dc] = Convert.ToDouble(keyValueArr[1].ToString());
                                break;

                            case "System.DateTime":
                                dr[dc] = Convert.ToDateTime(keyValueArr[1].ToString());
                                break;

                            case "System.Int64":    // Added in case datatable column is of integer type and Integer having null value.
                            case "System.Int32":
                                if (String.IsNullOrEmpty(keyValueArr[1].ToString()))
                                    dr[dc] = DBNull.Value;
                                else
                                    dr[dc] = int.Parse(keyValueArr[1].ToString());
                                break;


                        }
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
