using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.Data.SqlClient
{
    /// <summary>
    /// SQL Extensions
    /// </summary>
    /// <remarks></remarks>
    public static class SQLExtensions
    {
        /// <summary>
        /// Submits the changes using SQLBulkCopy
        /// </summary>
        /// <typeparam name="T_Entity">The type of the _ entity.</typeparam>
        /// <param name="records">The records.</param>
        /// <param name="table">The table.</param>
        /// <param name="ConnectionString">The connection string.</param>
        /// <remarks></remarks>
        public static void SQLBulkInsert<T_Entity>(IEnumerable<T_Entity> records, string table, string ConnectionString) where T_Entity : class
        {
            if (records.Count() == 0) return;

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                //must add TableLock if we plan to save in paralell
                SqlBulkCopy bc = new SqlBulkCopy(con, SqlBulkCopyOptions.TableLock, null) { BulkCopyTimeout = 3600, BatchSize = 0, DestinationTableName = table };
                //SqlBulkCopy bc = new SqlBulkCopy(con) { BulkCopyTimeout = 3600, BatchSize = 0, DestinationTableName = table };

                AddColumnMappings(records.First(), table, bc, ConnectionString);
                CheckInfinite(records);

                bc.WriteToServer(records.AsDataReader());
            }
        }

        /// <summary>
        /// SQLs the bulk off load.
        /// </summary>
        /// <typeparam name="T_Entity">The type of the _ entity.</typeparam>
        /// <param name="sqlDelete">The SQL delete.</param>
        /// <param name="records">The records.</param>
        /// <param name="file">The file.</param>
        /// <param name="ConnectionString">The connection string.</param>
        /// <remarks></remarks>
        public static void SQLBulkOffLoad<T_Entity>(string sqlDelete, IEnumerable<T_Entity> records, string file, string ConnectionString) where T_Entity : class
        {
            if (records.Count() == 0) return;

            var positions = from p in records select p;
            // string.Join("\t", positions.ToArray());


        }
        /// <summary>
        /// SQL bulk insert with delete. Single Transaction
        /// </summary>
        /// <typeparam name="T_Entity">The type of the _ entity.</typeparam>
        /// <param name="sqlDelete">The SQL delete.</param>
        /// <param name="records">The records.</param>
        /// <param name="table">The table.</param>
        /// <param name="ConnectionString">The connection string.</param>
        /// <remarks></remarks>
        public static void SQLBulkInsertWithDelete<T_Entity>(string sqlDelete, IEnumerable<T_Entity> records, string table, string ConnectionString) where T_Entity : class
        {
            if (records.Count() == 0) return;

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                if (string.IsNullOrEmpty(sqlDelete) == false)
                {
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = sqlDelete;
                        cmd.CommandTimeout = 3600;
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }
                }

                //must add TableLock if we plan to save in paralell
                SqlBulkCopy bc = new SqlBulkCopy(con, SqlBulkCopyOptions.TableLock, null) { BulkCopyTimeout = 3600, BatchSize = 0, DestinationTableName = table };

                AddColumnMappings(records.First(), table, bc, ConnectionString);
                CheckInfinite(records);

                bc.WriteToServer(records.AsDataReader());
            }
        }
        /// <summary>
        /// Executes an SQL bulk delete.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="ConnectionString">The connection string.</param>
        /// <remarks></remarks>
        public static void SQLBulkDelete(string sql, string ConnectionString)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
        }

        /// <summary>
        /// Adds the column mappings.
        /// </summary>
        /// <typeparam name="T_Entity">The type of the _ entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="copy">The copy.</param>
        /// <remarks></remarks>
        public static void AddColumnMappings<T_Entity>(T_Entity entity, string table, SqlBulkCopy copy, string ConnectionString)
        {
            Type type = entity.GetType();
            PropertyInfo[] props = type.GetProperties();

            List<string> columnnames = new List<string>();

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = string.Format(@"Select [Name] As ColumnName From Sys.Columns Where Object_Id = Object_Id('{0}')", table);
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = System.Data.CommandType.Text;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string columnname = reader.GetString(reader.GetOrdinal("ColumnName"));
                        columnnames.Add(columnname);
                    }
                }
                con.Close();
            }

            foreach (PropertyInfo prop in props)
            {
                if (columnnames.Contains(prop.Name))
                    copy.ColumnMappings.Add(prop.Name, prop.Name);
            }
        }

        /// <summary>
        /// Checks the infinite.
        /// </summary>
        /// <typeparam name="T_Entity">The type of the _ entity.</typeparam>
        /// <param name="entities">The entities.</param>
        /// <remarks></remarks>
        public static void CheckInfinite<T_Entity>(IEnumerable<T_Entity> entities) where T_Entity : class
        {

            foreach (var entity in entities)
            {
                Type type = entity.GetType();
                var props = type.GetProperties();

                foreach (var prop in props)
                {
                    object value = prop.GetValue(entity, null);

                    if (value == null) continue;
                    if (value.GetType() == typeof(double) || value.GetType() == typeof(double?))
                    {
                        if (value != null)
                        {

                            if (double.IsNaN((double)value) || double.IsInfinity((double)value))
                            {
                                System.Diagnostics.Debug.Print("{0} = {1}", prop.Name, value);
                                prop.SetValue(entity, null, null);
                            }

                            //System.Diagnostics.Debug.Assert(!double.IsInfinity((double)value));
                            // System.Diagnostics.Debug.Assert(!double.IsNaN((double)value));
                        }

                    }
                }


            }
        }
    }
}
