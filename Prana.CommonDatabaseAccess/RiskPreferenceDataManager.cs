using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.DatabaseManager;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.CommonDatabaseAccess
{
    public class RiskPreferenceDataManager : IRiskPreferenceDataManager
    {
        static readonly object _locker = new object();
        private static int _errorNumber = 0;
        private static string _errorMessage = string.Empty;

        public SortedList<int, float> SetInerestRateFromDB(RiskPrefernece riskPreferences)
        {
            DataTable dt = GetDataTableSchema();
            SortedList<int, float> _interestRates = new SortedList<int, float>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetInterestRate";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        DataRow dr = dt.NewRow();
                        dr["Period"] = row[0];
                        dr["Rate"] = row[1];
                        if (row[0] != System.DBNull.Value && row[1] != System.DBNull.Value)
                        {
                            int key = int.MinValue;
                            int.TryParse(row[0].ToString(), out key);
                            float value = float.MinValue;
                            float.TryParse(row[1].ToString(), out value);
                            if (key != int.MinValue && value != float.MinValue && !_interestRates.ContainsKey(key))
                            {
                                _interestRates.Add(key, value);
                            }
                        }
                        dt.Rows.Add(dr);
                    }
                }
                DataSet ds = new DataSet("InterestRate");
                ds.Tables.Add(dt);
                riskPreferences.InterestRateTable = ds;
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
            return _interestRates;
        }

        private static DataTable GetDataTableSchema()
        {
            DataTable dt = new DataTable("InterestRates");
            try
            {
                dt.Columns.Add(new DataColumn("Period", typeof(int)));
                dt.Columns.Add(new DataColumn("Rate", typeof(float)));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
            return dt;
        }
        private static DataTable GetSymbolMappingDataTableSchema()
        {
            DataTable dt = new DataTable("SymbolMapping");
            try
            {
                dt.Columns.Add(new DataColumn("Symbol", typeof(string)));
                dt.Columns.Add(new DataColumn("PSSymbol", typeof(string)));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
            return dt;
        }

        public Dictionary<string, string> SetPSSymbolMappingFromDB(RiskPrefernece riskPreferences)
        {
            DataTable dt = GetSymbolMappingDataTableSchema();
            Dictionary<string, string> _dictPSSymbolMapping = new Dictionary<string, string>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetPSSymbolMapping";

            try
            {
                lock (_locker)
                {
                    using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                    {
                        while (reader.Read())
                        {
                            object[] row = new object[reader.FieldCount];
                            reader.GetValues(row);
                            DataRow dr = dt.NewRow();
                            dr["Symbol"] = row[0];
                            dr["PSSymbol"] = row[1];
                            dt.Rows.Add(dr);

                            if (row[0] != System.DBNull.Value && row[1] != System.DBNull.Value)
                            {
                                string symbol = row[0].ToString();
                                string PSSymbol = row[1].ToString();

                                if (symbol != string.Empty && PSSymbol != string.Empty)
                                {
                                    if (!_dictPSSymbolMapping.ContainsKey(symbol))
                                    {
                                        _dictPSSymbolMapping.Add(symbol, PSSymbol);
                                    }
                                }
                            }
                        }
                    }
                    DataSet ds = new DataSet("SymbolMapping");
                    ds.Tables.Add(dt);
                    riskPreferences.SymbolMappingTable = ds;
                }
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
            return _dictPSSymbolMapping;
        }
        public DataTable SetDefaultVolShockAdjFactorFromDB(RiskPrefernece riskPreferences)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetVolShockAdjFactorData_Default";

                DataSet dsVolShock = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                if (dsVolShock.Tables.Count > 0)
                {
                    riskPreferences.DtVolShockFactorDefault = dsVolShock.Tables[0];
                }
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
            return new DataTable();
        }

        public void DeleteInterestRateFromDB(int id)
        {
            object[] parameter = new object[1];

            try
            {
                parameter[0] = id;
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteInterestRate", parameter);
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
        }
        public string SaveInterestRatesToDB(DataTable dt)
        {
            string IRXml = string.Empty;
            try
            {
                MemoryStream stream = new MemoryStream();
                dt.WriteXml(stream);

                byte[] bytes = stream.ToArray();
                IRXml = System.Text.ASCIIEncoding.ASCII.GetString(bytes);

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SaveInterestRate";
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = IRXml
                });

                XMLSaveManager.AddOutErrorParameters(queryData);
                DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
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
            return _errorMessage;
        }
        public string SavePSSymbolMappingToDB(DataTable dt)
        {
            string IRXml = string.Empty;
            try
            {
                MemoryStream stream = new MemoryStream();
                dt.WriteXml(stream);

                byte[] bytes = stream.ToArray();
                IRXml = System.Text.ASCIIEncoding.ASCII.GetString(bytes);

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SavePSSymbolMapping";
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = IRXml
                });

                XMLSaveManager.AddOutErrorParameters(queryData);
                DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
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
            return _errorMessage;
        }
    }
}
