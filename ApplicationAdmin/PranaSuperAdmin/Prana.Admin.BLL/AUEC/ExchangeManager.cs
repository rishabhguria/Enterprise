#region Using namespaces

using Prana.BusinessObjects.CommonObjects;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Data;

#endregion

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for ExchangeManager.
    /// </summary>
    public class ExchangeManager
    {
        private ExchangeManager()
        {
        }

        #region Exchange Methods

        /// <summary>
        /// FillExchanges is a method to fill a object of <see cref="Exchange"/> class.
        /// </summary>
        /// <param name="row">Row of table in the form of a single dimentional array.</param>
        /// <param name="offSet">Offset value from where values of <see cref="Exchange"/> class starts in object array.</param>
        /// <returns>Object of filled <see cref="Exchange"/> class.</returns>
        /// <remarks>Consideration here is that parameter "row" is a array which contains a single row of any "reader". And, this row contains all values of Asset class. So, if the row only contains result from T_Exchange table then value of Offset would be zero ("0"), and, lets say the row contains value of Asset as well as any other table then we have to specify the offset from where the values of Exchange starts. Note: Sequence of Exchange class whould be always same as in this method.</remarks>		
        public static Exchange FillExchanges(object[] row, int offSet)
        {
            int ID = 0 + offSet;
            int FULL_NAME = 1 + offSet;
            int DISPLAY_NAME = 2 + offSet;
            int TIME_ZONE = 3 + offSet;
            int REGULAR_TRADING_STARTTIME = 4 + offSet;
            int REGULAR_TRADING_ENDTIME = 5 + offSet;
            int LUNCH_TIME_STARTTIME = 6 + offSet;
            int LUNCH_TIME_ENDTIME = 7 + offSet;
            int COUNTRY = 8 + offSet;
            int STATEID = 9 + offSet;
            int REGULAR_TIME = 10 + offSet;
            int LUNCH_TIME = 11 + offSet;
            int COUNTRY_FLAG_ID = 12 + offSet;
            int LOGO_ID = 13 + offSet;
            int EXCHANGE_IDENTIFIER = 14 + offSet;
            int TIMEZONE_OFFSET = 15 + offSet;

            Exchange exchange = new Exchange();
            try
            {
                if (row[ID] != System.DBNull.Value)
                {
                    exchange.ExchangeID = int.Parse(row[ID].ToString());
                }
                if (row[FULL_NAME] != System.DBNull.Value)
                {
                    exchange.Name = row[FULL_NAME].ToString();
                }
                if (row[DISPLAY_NAME] != System.DBNull.Value)
                {
                    exchange.DisplayName = row[DISPLAY_NAME].ToString();
                }
                if (row[TIME_ZONE] != System.DBNull.Value)
                {
                    exchange.TimeZone = row[TIME_ZONE].ToString();
                }
                if (row[REGULAR_TRADING_STARTTIME] != System.DBNull.Value)
                {
                    exchange.RegularTradingStartTime = DateTime.Parse(row[REGULAR_TRADING_STARTTIME].ToString());
                }
                if (row[REGULAR_TRADING_ENDTIME] != System.DBNull.Value)
                {
                    exchange.RegularTradingEndTime = DateTime.Parse(row[REGULAR_TRADING_ENDTIME].ToString());
                }
                if (row[LUNCH_TIME_STARTTIME] != System.DBNull.Value)
                {
                    exchange.LunchTimeStartTime = DateTime.Parse(row[LUNCH_TIME_STARTTIME].ToString());
                }

                if (row[LUNCH_TIME_ENDTIME] != System.DBNull.Value)
                {
                    exchange.LunchTimeEndTime = DateTime.Parse(row[LUNCH_TIME_ENDTIME].ToString());
                }
                if (row[COUNTRY] != System.DBNull.Value)
                {
                    exchange.Country = int.Parse(row[COUNTRY].ToString());
                }
                if (row[STATEID] != System.DBNull.Value)
                {
                    exchange.StateID = int.Parse(row[STATEID].ToString());
                }
                if (row[REGULAR_TIME] != System.DBNull.Value)
                {
                    exchange.RegularTimeCheck = int.Parse(row[REGULAR_TIME].ToString());
                }
                if (row[LUNCH_TIME] != System.DBNull.Value)
                {
                    exchange.LunchTimeCheck = int.Parse(row[LUNCH_TIME].ToString());
                }
                if (row[COUNTRY_FLAG_ID] != System.DBNull.Value)
                {
                    exchange.CountryFlagID = int.Parse(row[COUNTRY_FLAG_ID].ToString());
                }
                if (row[LOGO_ID] != System.DBNull.Value)
                {
                    exchange.LogoID = int.Parse(row[LOGO_ID].ToString());
                }
                if (row[EXCHANGE_IDENTIFIER] != System.DBNull.Value)
                {
                    exchange.ExchangeIdentifier = row[EXCHANGE_IDENTIFIER].ToString();
                }
                if (row[TIMEZONE_OFFSET] != System.DBNull.Value)
                {
                    exchange.TimeZoneOffSet = double.Parse(row[TIMEZONE_OFFSET].ToString());
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
            return exchange;
        }

        public static Exchange FillExchangesForTradingTicket(object[] row, int offSet)
        {
            int ID = 0 + offSet;
            int FULL_NAME = 1 + offSet;
            int DISPLAY_NAME = 2 + offSet;

            Exchange exchange = new Exchange();
            try
            {
                if (row[ID] != System.DBNull.Value)
                {
                    exchange.ExchangeID = int.Parse(row[ID].ToString());
                }
                if (row[FULL_NAME] != System.DBNull.Value)
                {
                    exchange.Name = row[FULL_NAME].ToString();
                }
                if (row[DISPLAY_NAME] != System.DBNull.Value)
                {
                    exchange.DisplayName = row[DISPLAY_NAME].ToString();
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
            return exchange;
        }

        /// <summary>
        /// Gets all <see cref="Exchange"/> in <see cref="Exchanges"/> collection.
        /// </summary>
        /// <returns>Object of <see cref="Exchanges"/>.</returns>
        public static Exchanges GetExchanges()
        {
            Exchanges exchanges = new Exchanges();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllExchanges";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        exchanges.Add(FillExchanges(row, 0));
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
            return exchanges;
        }

        /// <summary>
        /// Gets all master <see cref="Exchange"/> in <see cref="Exchanges"/> which are used in the AUECxchange.
        /// </summary>
        /// <returns>Object of <see cref="Exchanges"/>.</returns>
        public static Exchanges GetAUECCommonExchanges()
        {
            Exchanges exchanges = new Exchanges();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllCommonAUECExchanges";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        exchanges.Add(FillExchanges(row, 0));
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
            return exchanges;
        }

        /// <summary>
        /// Gets <see cref="exchange"/> for given assetID.
        /// </summary>
        /// <param name="exchangeID">ExchangeID for which <see cref="Exchange"/> is required.</param>
        /// <returns>Object of <see cref="Exchange"/>.</returns>
        public static Exchange GetExchange(int exchangeID)
        {
            Exchange exchange = null;

            Object[] parameter = new object[1];
            parameter[0] = (int)exchangeID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetExchangeByID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        exchange = FillExchanges(row, 0);
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
            return exchange;
        }

        /// <summary>
        /// This is a special method which brings all the exchanges from the table except the one supplied to it.
        /// </summary>
        /// <param name="exchangeID">This parameter is used to fetch the records from database.</param>
        /// <returns>Object of exchanges a collection of exchange object.</returns>
        public static Exchanges GetExchanges(int exchangeID)
        {
            Exchanges exchanges = new Exchanges();

            Object[] parameter = new object[1];
            parameter[0] = (int)exchangeID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetExchangesByID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        exchanges.Add(FillExchanges(row, 0));
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
            return exchanges;
        }

        public static Exchanges GetExchnagesByUserID(int userID)
        {
            Exchanges exchanges = new Exchanges();

            Object[] parameter = new object[1];
            parameter[0] = userID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetExchnagesByUserID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        exchanges.Add(FillExchangesForTradingTicket(row, 0));
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
            return exchanges;
        }

        public static bool DeleteExchange(int exchangeID)
        {
            bool result = false;

            try
            {
                object[] parameter = new object[1];
                parameter[0] = exchangeID;
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteExchange", parameter) > 0)
                {
                    result = true;
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
            return result;
        }

        #region  Exchange Form

        public static int SaveExchangeForm(Exchange exchange)
        {
            //			bool result = false;
            int res = int.MinValue;
            object[] parameter = new object[17];
            parameter[0] = exchange.ExchangeID;
            parameter[1] = exchange.Name;
            parameter[2] = exchange.DisplayName;
            parameter[3] = exchange.TimeZone;
            parameter[4] = exchange.LunchTimeStartTime;
            parameter[5] = exchange.LunchTimeEndTime;
            parameter[6] = exchange.RegularTradingStartTime;
            parameter[7] = exchange.RegularTradingEndTime;

            parameter[8] = exchange.RegularTimeCheck;
            parameter[9] = exchange.LunchTimeCheck;
            parameter[10] = exchange.Country;
            parameter[11] = exchange.StateID;
            parameter[12] = exchange.CountryFlagID;
            parameter[13] = exchange.LogoID;
            parameter[14] = exchange.ExchangeIdentifier;
            parameter[15] = exchange.TimeZoneOffSet;
            parameter[16] = int.MinValue;

            try
            {
                string arc = "P_InsertExchangeForm";
                res = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar(arc, parameter).ToString());
                //				if(res > 0)
                //				{
                //					result = true;
                //				}
                //				else
                //				{
                //					result = false;
                //				}
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
            //			return result;
            return res;
        }

        #endregion


        #endregion

        #region Holidays
        public static Holiday FillHolidays(object[] row, int offSet)
        {
            int holidayID = 0 + offSet;
            int date = 1 + offSet;
            int description = 2 + offSet;
            int exchangeID = 3 + offSet;

            Holiday holiday = new Holiday();
            try
            {
                if (row[holidayID] != null)
                {
                    holiday.HolidayID = int.Parse(row[holidayID].ToString());
                }
                if (row[date] != null)
                {
                    holiday.Date = DateTime.Parse(row[date].ToString());
                }
                if (row[description] != null)
                {
                    holiday.Description = row[description].ToString();
                }
                if (row[exchangeID] != null)
                {
                    holiday.ExchangeID = int.Parse(row[exchangeID].ToString());
                }
                else
                {
                    holiday.ExchangeID = int.MinValue;
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
            return holiday;
        }

        public static Holidays GetHolidays()
        {
            Holidays holidays = new Holidays();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetHolidays";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                //using(SqlDataReader reader = (SqlDataReader) db.ExecuteReader("P_GetHolidays", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        holidays.Add(FillHolidays(row, 0));
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
            return holidays;
        }

        public static Holidays GetHolidays(int exchangeID)
        {
            Holidays holidays = new Holidays();

            Object[] parameter = new object[1];
            parameter[0] = (int)exchangeID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetExchangeHolidays", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        holidays.Add(FillHolidays(row, 0));
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
            return holidays;
        }

        public static bool SaveExchangeHolidays(Holiday holiday)
        {
            bool result = false;
            //			foreach (Holiday holiday in holidays)
            //			{
            object[] parameter = new object[4];
            parameter[0] = holiday.ExchangeID;
            parameter[1] = holiday.HolidayID;
            parameter[2] = holiday.Date.ToShortDateString();
            parameter[3] = holiday.Description;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_InsertExchangeHolidays", parameter) > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
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
            //			}
            return result;
        }

        public static bool SaveExchangeHolidays(Holidays holidays)
        {
            bool result = false;
            foreach (Holiday holiday in holidays)
            {
                object[] parameter = new object[4];
                parameter[0] = holiday.ExchangeID;
                parameter[1] = holiday.HolidayID;
                parameter[2] = holiday.Date.ToShortDateString();
                parameter[3] = holiday.Description;

                try
                {
                    if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_InsertExchangeHolidays", parameter) > 0)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
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
            return result;
        }

        public static bool DeleteExchangeHoliday(int exchangeHolidayID)
        {
            bool result = false;

            try
            {
                object[] parameter = new object[1];
                parameter[0] = exchangeHolidayID;
                //parameter[1] = (deleteForceFully==true?1:0);
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteExchangeHoliday", parameter) > 0)
                {
                    result = true;
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
            return result;
        }



        public static bool CopyExchangeHolidays(Exchanges exchanges, int sourceExchangeID, string holidayID)
        {
            bool result = false;
            foreach (Exchange exchange in exchanges)
            {
                object[] parameter = new object[3];
                parameter[0] = exchange.ExchangeID;
                parameter[1] = sourceExchangeID;
                parameter[2] = holidayID;

                try
                {
                    if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_CopyExchangeHolidays", parameter) > 0)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
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
            return result;
        }

        #endregion

        #region Holidays Master
        public static Holiday GetHoliday(int holidayID)
        {
            Holiday holiday = new Holiday();

            object[] parameter = new object[1];
            parameter[0] = holidayID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetMasterHoliday", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        holiday = FillHolidays(row, 0);
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
            return holiday;
        }

        public static int SaveHoliday(Holiday holiday)
        {
            int result = int.MinValue;

            object[] parameter = new object[4];
            parameter[0] = holiday.HolidayID;
            parameter[1] = holiday.Date.ToShortDateString();
            parameter[2] = holiday.Description;
            parameter[3] = int.MinValue;
            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveMasterHolidays", parameter).ToString());
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
            return result;
        }

        public static bool DeleteHoliday(int holidayID, bool deleteForceFully)
        {
            bool result = false;

            try
            {
                object[] parameter = new object[2];
                parameter[0] = holidayID;
                parameter[1] = (deleteForceFully == true ? 1 : 0);
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteMasterHoliday", parameter) > 0)
                {
                    result = true;
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
            return result;
        }
        #endregion

        public static int SaveSMExchangeDetails(Exchange exchange, int exchangeID)
        {
            int result = 1;
            try
            {
                object[] parameter = new object[17];
                parameter[0] = exchangeID;
                parameter[1] = exchange.Name;
                parameter[2] = exchange.DisplayName;
                parameter[3] = exchange.TimeZone;
                parameter[4] = exchange.LunchTimeStartTime;
                parameter[5] = exchange.LunchTimeEndTime;
                parameter[6] = exchange.RegularTradingStartTime;
                parameter[7] = exchange.RegularTradingEndTime;

                parameter[8] = exchange.RegularTimeCheck;
                parameter[9] = exchange.LunchTimeCheck;
                parameter[10] = exchange.Country;
                parameter[11] = exchange.StateID;
                parameter[12] = exchange.CountryFlagID;
                parameter[13] = exchange.LogoID;
                parameter[14] = exchange.ExchangeIdentifier;
                parameter[15] = exchange.TimeZoneOffSet;
                parameter[16] = int.MinValue;

                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveSMExchangeDetails", parameter, ApplicationConstants.SMConnectionString);
            }
            catch (Exception ex)
            {
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
