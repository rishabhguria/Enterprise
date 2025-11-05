using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System;

namespace Prana.DatabaseManager.EnterpriseLibrary
{
    public class DatabaseManager : IDatabaseManager
    {
        private static readonly int _miscellanousTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["MiscellanousTimeout"]);
        public DataSet ExecuteDataSet(string spName, object[] parameter, string connectionName = "", int timeout = int.MinValue)
        {
            Database db;
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            if (string.IsNullOrWhiteSpace(connectionName))
            {
                db = factory.CreateDefault();
            }
            else
            {
                db = factory.Create(connectionName);
            }

            DbCommand cmd = db.GetStoredProcCommand(spName, parameter);

            if (timeout != int.MinValue)
                cmd.CommandTimeout = timeout;

            return db.ExecuteDataSet(cmd);
        }

        public DataSet ExecuteDataSet(QueryData databaseHelper, string connectionName = "")
        {
            Database db;
            DbCommand dbCommand;
            InputProcessing(databaseHelper, connectionName, out db, out dbCommand);
            dbCommand.CommandTimeout = _miscellanousTimeout;

            DataSet result = db.ExecuteDataSet(dbCommand);

            UpdateOutParameters(databaseHelper.DictionaryDatabaseParameter, dbCommand.Parameters);

            return result;
        }

        public object ExecuteScalar(string spName, object[] parameter, string connectionName = "")
        {
            Database db;
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            if (string.IsNullOrWhiteSpace(connectionName))
            {
                db = factory.CreateDefault();
            }
            else
            {
                db = factory.Create(connectionName);
            }
            return db.ExecuteScalar(spName, parameter);
        }

        public object ExecuteScalarWithTimeOut(string spName, object[] parameter, string connectionName = "")
        {
            Database db;
            DatabaseProviderFactory factory = new DatabaseProviderFactory();

            if (string.IsNullOrWhiteSpace(connectionName))
            {
                db = factory.CreateDefault();
            }
            else
            {
                db = factory.Create(connectionName);
            }

            DbCommand cmd = db.GetStoredProcCommand(spName, parameter);

            cmd.CommandTimeout = 600;

            return db.ExecuteScalar(cmd);
        }

        public object ExecuteScalar(QueryData databaseHelper, string connectionName = "")
        {
            Database db;
            DbCommand dbCommand;
            InputProcessing(databaseHelper, connectionName, out db, out dbCommand);

            object result = db.ExecuteScalar(dbCommand);

            UpdateOutParameters(databaseHelper.DictionaryDatabaseParameter, dbCommand.Parameters);

            return result;
        }

        public IDataReader ExecuteReader(string spName, object[] parameter, string connectionName = "", int timeout = int.MinValue)
        {
            Database db;
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            if (string.IsNullOrWhiteSpace(connectionName))
            {
                db = factory.CreateDefault();
            }
            else
            {
                db = factory.Create(connectionName);
            }

            DbCommand cmd = db.GetStoredProcCommand(spName, parameter);

            if (timeout != int.MinValue)
                cmd.CommandTimeout = timeout;

            return db.ExecuteReader(cmd);
        }

        public IDataReader ExecuteReader(QueryData databaseHelper, string connectionName = "")
        {
            Database db;
            DbCommand dbCommand;
            InputProcessing(databaseHelper, connectionName, out db, out dbCommand);

            IDataReader result = db.ExecuteReader(dbCommand);

            UpdateOutParameters(databaseHelper.DictionaryDatabaseParameter, dbCommand.Parameters);

            return result;
        }

        public int ExecuteNonQuery(string spName, object[] parameter, string connectionName = "", int timeout = int.MinValue)
        {
            Database db;
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            if (string.IsNullOrWhiteSpace(connectionName))
            {
                db = factory.CreateDefault();
            }
            else
            {
                db = factory.Create(connectionName);
            }

            DbCommand cmd = db.GetStoredProcCommand(spName, parameter);

            if (timeout != int.MinValue)
                cmd.CommandTimeout = timeout;

            return db.ExecuteNonQuery(cmd);
        }

        public int ExecuteNonQuery(QueryData databaseHelper, string connectionName = "", DbTransaction transaction = null)
        {
            Database db;
            DbCommand dbCommand;
            InputProcessing(databaseHelper, connectionName, out db, out dbCommand);

            int result = 0;

            if (transaction == null)
                result = db.ExecuteNonQuery(dbCommand);
            else
                result = db.ExecuteNonQuery(dbCommand, transaction);

            UpdateOutParameters(databaseHelper.DictionaryDatabaseParameter, dbCommand.Parameters);

            return result;
        }

        public void LoadDataSet(string storedProcedureName, DataSet dataSet, string[] tableNames, object[] parameterValues, string connectionName = "")
        {
            Database db;
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            if (string.IsNullOrWhiteSpace(connectionName))
            {
                db = factory.CreateDefault();
            }
            else
            {
                db = factory.Create(connectionName);
            }

            db.LoadDataSet(storedProcedureName, dataSet, tableNames, parameterValues);
        }

        #region Private Methods
        private void InputProcessing(QueryData databaseHelper, string connectionName, out Database db, out DbCommand dbCommand)
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            if (string.IsNullOrWhiteSpace(connectionName))
            {
                db = factory.CreateDefault();
            }
            else
            {
                db = factory.Create(connectionName);
            }

            if (!string.IsNullOrWhiteSpace(databaseHelper.StoredProcedureName))
            {
                dbCommand = db.GetStoredProcCommand(databaseHelper.StoredProcedureName);
            }
            else
            {
                dbCommand = db.GetSqlStringCommand(databaseHelper.Query);
            }

            if (databaseHelper.CommandTimeout > 0)
            {
                dbCommand.CommandTimeout = databaseHelper.CommandTimeout;
            }

            foreach (KeyValuePair<string, DatabaseParameter> databaseParameter in databaseHelper.DictionaryDatabaseParameter)
            {
                if (databaseParameter.Value.IsOutParameter)
                {
                    db.AddOutParameter(dbCommand, databaseParameter.Value.ParameterName, databaseParameter.Value.ParameterType, databaseParameter.Value.OutParameterSize);
                }
                else
                {
                    db.AddInParameter(dbCommand, databaseParameter.Value.ParameterName, databaseParameter.Value.ParameterType, databaseParameter.Value.ParameterValue);
                }
            }
        }

        private void UpdateOutParameters(Dictionary<string, DatabaseParameter> queryParameters, DbParameterCollection commandParameters)
        {
            for (int i = 0; i < commandParameters.Count; i++)
            {
                if (commandParameters[i].Direction == ParameterDirection.Output)
                    queryParameters[commandParameters[i].ParameterName].ParameterValue = commandParameters[i].Value;
            }
        }

        public DbConnection CreateConnection()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            Database db = factory.CreateDefault();
            return db.CreateConnection();
        }
        #endregion
    }
}
