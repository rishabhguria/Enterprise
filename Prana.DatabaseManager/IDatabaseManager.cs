using System.Data;
using System.Data.Common;

namespace Prana.DatabaseManager
{
    public interface IDatabaseManager
    {
        DataSet ExecuteDataSet(string spName, object[] parameter, string connectionName = "", int timeout = int.MinValue);

        DataSet ExecuteDataSet(QueryData databaseHelper, string connectionName = "");

        object ExecuteScalar(string spName, object[] parameter, string connectionName = "");

        object ExecuteScalarWithTimeOut(string spName, object[] parameter, string connectionName = "");


        object ExecuteScalar(QueryData databaseHelper, string connectionName = "");

        IDataReader ExecuteReader(string spName, object[] parameter, string connectionName = "", int timeout = int.MinValue);

        IDataReader ExecuteReader(QueryData databaseHelper, string connectionName = "");

        int ExecuteNonQuery(string spName, object[] parameter, string connectionName = "", int timeout = int.MinValue);

        int ExecuteNonQuery(QueryData databaseHelper, string connectionName = "", DbTransaction transaction = null);

        void LoadDataSet(string storedProcedureName, DataSet dataSet, string[] tableNames, object[] parameterValues, string connectionName = "");

        DbConnection CreateConnection();
    }
}
