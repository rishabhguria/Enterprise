using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.DatabaseManager;
using Prana.LogManager;
using Prana.Utilities.XMLUtilities;
using System;
using System.Data;
using System.IO;

namespace Prana.OptionCalculator.Common
{
    public class OptionModelDataManager
    {
        private static int _errorNumber = 0;
        private static string _errorMessage = string.Empty;

        public static DataSet GetOptionModelUserDataFromDB(string listSymbols, bool fetchZeroPositionData)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetOptionModelUserData";
                queryData.CommandTimeout = 180;
                queryData.DictionaryDatabaseParameter.Add("@listSymbols", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@listSymbols",
                    ParameterType = DbType.String,
                    ParameterValue = listSymbols
                });
                queryData.DictionaryDatabaseParameter.Add("@fetchZeroPositionData", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@fetchZeroPositionData",
                    ParameterType = DbType.Boolean,
                    ParameterValue = fetchZeroPositionData
                });

                using (DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData))
                {
                    if (ds.Tables.Count > 0)
                    {
                        return ds;
                    }
                }
            }
            #region Catch
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
            #endregion

            return null;
        }

        public static string SaveOptionModelUserDataToDB(string xml)
        {
            try
            {
                int affectedPositions = 0;

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SaveOptionModelUserData";
                queryData.CommandTimeout = 300;
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = xml
                });

                XMLSaveManager.AddOutErrorParameters(queryData);
                affectedPositions = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);

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

        public static string SaveOptionModelUserDataToDB(DataTable dtOMI)
        {
            int affectedPositions = 0;
            string optModelXml = string.Empty;
            try
            {

                dtOMI.DataSet.DataSetName = "ArrayOfUserOptModelInput";
                dtOMI.TableName = "UserOptModelInput";
                MemoryStream stream = new MemoryStream();
                dtOMI.WriteXml(stream);

                byte[] bytes = stream.ToArray();
                optModelXml = System.Text.ASCIIEncoding.ASCII.GetString(bytes);

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SaveOptionModelUserData";
                queryData.CommandTimeout = 300;
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = optModelXml
                });

                XMLSaveManager.AddOutErrorParameters(queryData);
                affectedPositions = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);

                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
            }
            #region Catch
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
            #endregion
            return _errorMessage;
        }

        public static System.Data.DataSet GetOptionModelTableFromDB()
        {
            DataSet ds = new DataSet();
            //Database db = DatabaseFactory.CreateDatabase();

            //try
            //{
            //    ds = db.ExecuteDataSet("P_GetOptionModelUserData");
            //}
            //#region Catch
            //catch (Exception ex)
            //{
            //    // Invoke our policy that is responsible for making sure no secure information
            //    // gets out of our layer.
            //    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

            //    if (rethrow)
            //    {
            //        throw;
            //    }
            //}
            //#endregion
            return ds;
            ////return ds.Tables[0];
        }


        public static LiveFeedPreferences GetOMIPrefDataFromDB()
        {
            LiveFeedPreferences preferences = new LiveFeedPreferences();

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetOMIPreferences";
                queryData.CommandTimeout = 300;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {

                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row[0] != System.DBNull.Value)
                        {
                            preferences.SelectedFeedPrice = (SelectedFeedPrice)Int32.Parse(row[0].ToString());
                        }
                        if (row[1] != System.DBNull.Value)
                        {
                            preferences.OptionSelectedFeedPrice = (SelectedFeedPrice)Int32.Parse(row[1].ToString());
                        }
                        if (row[2] != System.DBNull.Value)
                        {
                            preferences.UseClosingMark = bool.Parse(row[2].ToString());
                        }
                        if (row[3] != System.DBNull.Value)
                        {
                            preferences.OverrideWithOptions = (SelectedFeedPrice)Convert.ToInt32(row[3].ToString());
                        }
                        if (row[4] != System.DBNull.Value)
                        {
                            preferences.OverrideWithOthers = (SelectedFeedPrice)Convert.ToInt32(row[4].ToString());
                        }
                        if (row[5] != System.DBNull.Value)
                        {
                            preferences.UseDefaultDelta = bool.Parse(row[5].ToString());
                        }
                        if (row[6] != System.DBNull.Value)
                        {
                            preferences.OverrideConditionOptions = (NumericConditionOperator)Convert.ToInt32(row[6].ToString());
                        }
                        if (row[7] != System.DBNull.Value)
                        {
                            preferences.OverrideConditionOthers = (NumericConditionOperator)Convert.ToInt32(row[7].ToString());
                        }
                        if (row[8] != System.DBNull.Value)
                        {
                            preferences.PriceBarOptions = Convert.ToDecimal(row[8].ToString());
                        }
                        if (row[9] != System.DBNull.Value)
                        {
                            preferences.PriceBarOthers = Convert.ToDecimal(row[9].ToString());
                        }
                        if (row[10] != System.DBNull.Value)
                        {
                            preferences.OverrideCheckOptions = (SelectedFeedPrice)Convert.ToInt32(row[10].ToString());
                        }
                        if (row[11] != System.DBNull.Value)
                        {
                            preferences.OverrideCheckOthers = (SelectedFeedPrice)Convert.ToInt32(row[11].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return preferences;
        }

        public static void SaveOMIPrefDataintoDB(LiveFeedPreferences Prferences)
        {

            object[] parameters = new object[12];

            parameters[0] = Prferences.SelectedFeedPrice;
            parameters[1] = Prferences.OptionSelectedFeedPrice;
            parameters[2] = (int)Prferences.OverrideWithOptions;
            parameters[3] = (int)Prferences.OverrideWithOthers;
            parameters[4] = Prferences.UseClosingMark;
            parameters[5] = Prferences.UseDefaultDelta;
            parameters[6] = (int)Prferences.OverrideConditionOptions;
            parameters[7] = (int)Prferences.OverrideConditionOthers;
            parameters[8] = Prferences.PriceBarOptions;
            parameters[9] = Prferences.PriceBarOthers;
            parameters[10] = Prferences.OverrideCheckOptions;
            parameters[11] = Prferences.OverrideCheckOthers;

            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveOMIPreferences", parameters).ToString();

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        public static DataSet GetPIDataForCustomSymbols()
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetCustomPIData";
                queryData.CommandTimeout = 180;
                using (DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData))
                {
                    if (ds.Tables.Count > 0)
                    {
                        return ds;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        public static DataSet DeleteSymbolfromPI(string symbol)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_DeleteSymbolfromPI";
                queryData.CommandTimeout = 180;
                queryData.DictionaryDatabaseParameter.Add("@listSymbol", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@listSymbol",
                    ParameterType = DbType.String,
                    ParameterValue = symbol
                });

                using (DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData))
                {
                    if (ds.Tables.Count > 0)
                    {
                        return ds;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }
        //return PrefereneceData;
        //internal static DataSet GetHistoricalSymbolsFromDB()
        //{
        //    DataSet ds = new DataSet();
        //   Database db = DatabaseFactory.CreateDatabase();
        //   try
        //   {
        //       ds = db.ExecuteDataSet("P_GetHistoricalSymbolsForOMI");
        //   }
        //   catch (Exception ex)
        //   {
        //       bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //       if (rethrow)
        //       {
        //           throw;
        //       }
        //   }
        //   return ds;
        //}
    }
}
