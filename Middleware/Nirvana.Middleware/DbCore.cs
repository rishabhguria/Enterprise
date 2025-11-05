using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Nirvana.Middleware
{
    public static class DbCore
    {
        private static SqlClientFactory factory = SqlClientFactory.Instance;

        public static List<T> TextAccessGenericlist<T>(Func<IDataReader, T> make, string constr, string sql, DbParameter[] parameters = null)
        {
            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = constr;

                using (var command = factory.CreateCommand())
                {
                    command.Connection = connection;

                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;

                    if (parameters != null)
                        foreach (DbParameter parameter in parameters)
                            command.Parameters.Add(parameter);

                    connection.Open();

                    List<T> list = new List<T>();
                    DbDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                        list.Add(make(reader));

                    return list;
                }
            }
        }
    }

    public class QueryParams
    {
        public string Query { get; set; }

        public SqlParameter[] Params { get; set; }
    }
}
