using Prana.LogManager;
using System;
using System.Data;

namespace Prana.LiveFeed.UI
{
    public class AnalyticsDataManager
    {

        private static readonly AnalyticsDataManager instance = new AnalyticsDataManager();
        private AnalyticsDataManager()
        {
        }

        public static AnalyticsDataManager GetInstance()
        {
            return instance;
        }

        #region Interest Rates Data Manager
        public void GetInterestRates(int userID, DataTable interestRateTable)
        {
            object[] param = new object[1];
            param[0] = userID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetInterestRates", param))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        DataRow interestRateRow = interestRateTable.NewRow();
                        interestRateRow["Date"] = row[0];
                        interestRateRow["Auto"] = row[1];
                        interestRateRow["Manual"] = row[2];
                        interestRateTable.Rows.Add(interestRateRow);
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
        }

        public void SaveInterestRates(int userID, DataTable interestRateTable)
        {
            try
            {
                object[] param = new object[1];
                param[0] = userID;

                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteInterestRates", param);

                //using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_DeleteInterestRates", param)) ;
                foreach (DataRow row in interestRateTable.Rows)
                {
                    object[] parameter = new object[4];
                    parameter[0] = userID;
                    parameter[1] = row["Date"].ToString();
                    if (row["Auto"].ToString() == string.Empty)
                    {
                        parameter[2] = 0.0;
                    }
                    else
                    {
                        parameter[2] = double.Parse(row["Auto"].ToString());
                    }
                    if (row["Auto"].ToString() == string.Empty)
                    {
                        parameter[3] = 0.0;
                    }
                    else
                    {
                        parameter[3] = double.Parse(row["Manual"].ToString());
                    }
                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveInterestRates", parameter);

                    //using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_SaveInterestRates", parameter)) ;
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
        }

        public void GetUniversalInterestRates(DataTable interestRateTable)
        {
            object[] param = new object[1];
            param[0] = int.MinValue;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetInterestRates", param))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        DataRow interestRateRow = interestRateTable.NewRow();
                        interestRateRow["Date"] = row[0];
                        interestRateRow["Auto"] = row[1];
                        interestRateRow["Manual"] = row[2];
                        interestRateTable.Rows.Add(interestRateRow);
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
        }

        public void SaveUniversalInterestRates(DataTable interestRateTable)
        {
            try
            {
                object[] param = new object[1];
                param[0] = int.MinValue;

                //using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_DeleteInterestRates", param)) ;

                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteInterestRates", param);

                foreach (DataRow row in interestRateTable.Rows)
                {
                    object[] parameter = new object[4];
                    parameter[0] = int.MinValue;
                    parameter[1] = row["Date"].ToString();
                    if (row["Auto"].ToString() == string.Empty)
                    {
                        parameter[2] = 0.0;
                    }
                    else
                    {
                        parameter[2] = double.Parse(row["Auto"].ToString());
                    }
                    if (row["Auto"].ToString() == string.Empty)
                    {
                        parameter[3] = 0.0;
                    }
                    else
                    {
                        parameter[3] = double.Parse(row["Manual"].ToString());
                    }
                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveInterestRates", parameter);
                    //using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_SaveInterestRates", parameter)) ;
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


        }

        #endregion


        #region Volatiltiy Data Manager
        public void SaveSymbolVolatilityData(int userID, DataTable volatilityTable, string underLyingSymbol)
        {
            try
            {
                //no need to delete as saving updates data for a given security
                //using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_DeleteInterestRates", param));

                foreach (DataRow row in volatilityTable.Rows)
                {
                    object[] paramCall = new object[7];
                    paramCall[0] = row["SymbolCall"];
                    paramCall[1] = "C";
                    paramCall[2] = underLyingSymbol;
                    paramCall[3] = double.Parse(row["Strike"].ToString());
                    paramCall[4] = double.Parse(row["Implied Call Vol."].ToString());
                    paramCall[5] = double.Parse(row["User Call Vol."].ToString());
                    paramCall[6] = userID;
                    //using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_SaveVolatilityData", paramCall)) ;
                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveVolatilityData", paramCall);

                    object[] paramPut = new object[7];
                    paramPut[0] = row["SymbolPut"];
                    paramPut[1] = "P";
                    paramPut[2] = underLyingSymbol;
                    paramPut[3] = double.Parse(row["Strike"].ToString());
                    paramPut[4] = double.Parse(row["Implied Put Vol."].ToString());
                    paramPut[5] = double.Parse(row["User Put Vol."].ToString());
                    paramPut[6] = userID;
                    //using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_SaveVolatilityData", paramPut)) ;
                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveVolatilityData", paramPut);


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



        }

        public void GetCompleteVolatilityData(int userID, DataTable volatilityTable, string symbol)
        {
            object[] param = new object[2];
            param[0] = symbol;
            param[1] = userID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetVolatilityData", param))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        DataRow volatilityRow = volatilityTable.NewRow();
                        volatilityRow["Strike"] = row[0];
                        volatilityRow["Implied Volatility"] = row[1];
                        volatilityRow["User Volatility"] = row[2];
                        volatilityTable.Rows.Add(volatilityRow);
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
        }

        public double GetUserVolatilityData(int userID, string symbol)
        {
            double UserVolatility = 0.0;
            object[] param = new object[2];
            param[0] = symbol;
            param[1] = userID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetVolatilityData", param))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        //DataRow volatilityRow = volatilityTable.NewRow();
                        //volatilityRow["Strike"] = row[0];
                        //volatilityRow["Implied Volatility"] = row[1];
                        UserVolatility = Convert.ToDouble(row[5].ToString());
                        //volatilityTable.Rows.Add(volatilityRow);
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
            return UserVolatility;
        }

        #endregion



    }
}
