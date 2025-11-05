using Castle.Windsor;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Prana.DatabaseManager
{
    public static class DatabaseManager
    {
        private static IDatabaseManager _databaseManager;

        public static void Initialize(IWindsorContainer container)
        {
            _databaseManager = container.Resolve<IDatabaseManager>();
        }

        #region Library Dependent Methods
        public static DataSet ExecuteDataSet(string spName, object[] parameter, string connectionName = "", int timeout = int.MinValue)
        {
            return _databaseManager.ExecuteDataSet(spName, parameter, connectionName, timeout);
        }

        public static DataSet ExecuteDataSet(QueryData databaseHelper, string connectionName = "")
        {
            return _databaseManager.ExecuteDataSet(databaseHelper, connectionName);
        }

        public static object ExecuteScalar(string spName, object[] parameter, string connectionName = "")
        {
            return _databaseManager.ExecuteScalar(spName, parameter, connectionName);
        }

        public static object ExecuteScalarWithTimeOut(string spName, object[] parameter, string connectionName = "")
        {
            return _databaseManager.ExecuteScalarWithTimeOut(spName, parameter, connectionName);
        }


        public static object ExecuteScalar(QueryData databaseHelper, string connectionName = "")
        {
            return _databaseManager.ExecuteScalar(databaseHelper, connectionName);
        }

        public static IDataReader ExecuteReader(string spName, object[] parameter, string connectionName = "", int timeout = int.MinValue)
        {
            return _databaseManager.ExecuteReader(spName, parameter, connectionName, timeout);
        }

        public static IDataReader ExecuteReader(QueryData databaseHelper, string connectionName = "")
        {
            return _databaseManager.ExecuteReader(databaseHelper, connectionName);
        }

        public static int ExecuteNonQuery(string spName, object[] parameter, string connectionName = "", int timeout = int.MinValue)
        {
            return _databaseManager.ExecuteNonQuery(spName, parameter, connectionName, timeout);
        }

        public static int ExecuteNonQuery(QueryData databaseHelper, string connectionName = "", DbTransaction transaction = null)
        {
            return _databaseManager.ExecuteNonQuery(databaseHelper, connectionName, transaction);
        }

        public static void LoadDataSet(string storedProcedureName, DataSet dataSet, string[] tableNames, object[] parameterValues, string connectionName = "")
        {
            _databaseManager.LoadDataSet(storedProcedureName, dataSet, tableNames, parameterValues, connectionName);
        }

        public static DbConnection CreateConnection()
        {
            return _databaseManager.CreateConnection();
        }
        #endregion

        #region System.Data Methods
        public static SqlConnection CreateConnection(string connString)
        {
            return new SqlConnection(connString);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static SqlCommand CreateSqlCommand(ref SqlConnection conn, string storedProcedure, int timeout = int.MinValue, SqlInfoMessageEventHandler infoHandler = null)
        {
            conn = (SqlConnection)CreateConnection();

            if (infoHandler != null)
                conn.InfoMessage += infoHandler;

            SqlCommand cmd = new SqlCommand(storedProcedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            if (timeout != int.MinValue)
                cmd.CommandTimeout = timeout;

            return cmd;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static SqlDataAdapter CreateSqlDataAdapter(ref SqlConnection conn, string query = "")
        {
            conn = (SqlConnection)CreateConnection();

            SqlDataAdapter da;

            if (string.IsNullOrEmpty(query))
                da = new SqlDataAdapter();
            else
                da = new SqlDataAdapter(query, conn);

            new SqlCommandBuilder(da);

            return da;
        }

        public static SqlDataAdapter CreateSqlDataAdapter(SqlCommand cmd)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            new SqlCommandBuilder(da);

            return da;
        }

        public static SqlTransaction BeginTransaction(SqlConnection conn)
        {
            return conn.BeginTransaction();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static void Update(string query, DataTable dataTable)
        {
            SqlConnection conn = null;

            using (SqlDataAdapter sqlDataAdapter = CreateSqlDataAdapter(ref conn, query))
            {
                conn.Open();
                sqlDataAdapter.Update(dataTable);
            }
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <param name="storedProcedure">The SQL.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static SqlParameterCollection GetParameters(string storedProcedure)
        {
            SqlConnection conn = null;

            using (SqlCommand cmd = CreateSqlCommand(ref conn, storedProcedure))
            {
                conn.Open();

                SqlCommandBuilder.DeriveParameters(cmd);

                return cmd.Parameters;
            }
        }
        #endregion
    }
}
