using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Nirvana.TestAutomation.Utilities
{
    public static class DataTableComparer
    {
        public static bool AreTablesEqual(DataTable table1, DataTable table2)
        {
            try
            {
                if (!AreSchemasEqual(table1, table2))
                {
                    return false;
                }

                if (!AreDataEqual(table1, table2))
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return true;
        }

        private static bool AreSchemasEqual(DataTable table1, DataTable table2)
        {
            if (table1.Columns.Count != table2.Columns.Count)
            {
                return false;
            }

            foreach (DataColumn column1 in table1.Columns)
            {
                if (!table2.Columns.Contains(column1.ColumnName))
                {
                    return false;
                }

                DataColumn column2 = table2.Columns[column1.ColumnName];
                if (column1.DataType != column2.DataType)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool AreDataEqual(DataTable table1, DataTable table2)
        {
            if (table1.Rows.Count != table2.Rows.Count || table1.Columns.Count != table2.Columns.Count)
            {
                return false;
            }

            foreach (DataColumn column in table1.Columns)
            {
                if (!table2.Columns.Contains(column.ColumnName))
                {
                    return false;
                }
            }

            string sortString = string.Join(", ", table1.Columns.Cast<DataColumn>().Select(col => col.ColumnName));
            DataView view1 = new DataView(table1);
            DataView view2 = new DataView(table2);
            view1.Sort = sortString;
            view2.Sort = sortString;

            for (int i = 0; i < view1.Count; i++)
            {
                DataRowView rowView1 = view1[i];
                DataRowView rowView2 = view2[i];

                foreach (DataColumn column in table1.Columns)
                {
                    string columnName = column.ColumnName;

                    if (!rowView1[columnName].Equals(rowView2[columnName]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
