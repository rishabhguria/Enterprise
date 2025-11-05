using System.Collections.Generic;

namespace Prana.DatabaseManager
{
    public class QueryData
    {
        private string _connectionName;
        public string ConnectionName
        {
            get { return _connectionName; }
            set { _connectionName = value; }
        }

        private string _storedProcedureName;
        public string StoredProcedureName
        {
            get { return _storedProcedureName; }
            set { _storedProcedureName = value; }
        }

        private string _query;
        public string Query
        {
            get { return _query; }
            set { _query = value; }
        }

        private int _commandTimeout;
        public int CommandTimeout
        {
            get { return _commandTimeout; }
            set { _commandTimeout = value; }
        }

        private Dictionary<string, DatabaseParameter> _dictionaryDatabaseParameter = new Dictionary<string, DatabaseParameter>();
        public Dictionary<string, DatabaseParameter> DictionaryDatabaseParameter
        {
            get { return _dictionaryDatabaseParameter; }
            set { _dictionaryDatabaseParameter = value; }
        }
    }
}
