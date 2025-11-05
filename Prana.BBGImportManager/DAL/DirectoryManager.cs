using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Configuration;
using System.Data;

namespace Prana.BBGImportManager
{
    internal class DirectoryManager
    {
        public static int SaveDataIntoDB(string sourceFilePath, string spName)
        {
            int result = int.MinValue;

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = spName;
                queryData.CommandTimeout = int.Parse(ConfigurationManager.AppSettings["BBGFileImportDBTimeInterval"].ToString());
                queryData.DictionaryDatabaseParameter.Add("@filePath", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@filePath",
                    ParameterType = DbType.String,
                    ParameterValue = sourceFilePath
                });

                result = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            return result;
        }
    }
}