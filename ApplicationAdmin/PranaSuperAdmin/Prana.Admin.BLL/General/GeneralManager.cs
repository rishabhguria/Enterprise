#region Using

using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Data;
#endregion

namespace Prana.Admin.BLL
{
    /// <summary>
    /// This is a general manager class which will contain micelenaous methods.
    /// </summary>
    public class GeneralManager
    {
        private GeneralManager()
        {
        }

        #region Country

        /// <summary>
        /// Fills the row of country to <see cref="Country"/> object.
        /// </summary>
        /// <param name="row">Datarow to be filled.</param>
        /// <param name="offset">offset</param>
        /// <returns>Object of <see cref="Country"/></returns>
        private static Country FillCountry(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            Country country = null;
            try
            {
                if (row != null)
                {
                    country = new Country();
                    int COUNTRYID = offset + 0;
                    int COUNTRYNAME = offset + 1;

                    country.CountryID = Convert.ToInt32(row[COUNTRYID]);
                    country.Name = Convert.ToString(row[COUNTRYNAME]);
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
            return country;
        }

        /// <summary>
        /// Gets all <see cref="Countries"/> available in database.
        /// </summary>
        /// <returns>Object mapping of <see cref="Countries"/> in database.</returns>
        public static Countries GetCountries()
        {
            Countries countries = new Countries();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetCountries";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        countries.Add(FillCountry(row, 0));
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
            return countries;
        }

        /// <summary>
        /// Gets <see cref="Country"/> corresponding to specified ID.
        /// </summary>
        /// <param name="countryID">ID for which <see cref="Country"/> is sought.</param>
        /// <returns>Object of <see cref="Country"/></returns>
        public static Country GetCountry(int countryID, string countryName = "")
        {
            Country country = new Country();

            Object[] parameter = new object[2];
            parameter[0] = countryID;
            parameter[1] = countryName;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCountry", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        country = FillCountry(row, 0);
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
            return country;
        }

        #endregion

        #region State

        /// <summary>
        /// Fills the row of state to <see cref="State"/> object.
        /// </summary>
        /// <param name="row">Datarow to be filled.</param>
        /// <param name="offset">offset</param>
        /// <returns>Object of <see cref="State"/></returns>
        private static State FillState(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            State state = null;
            try
            {
                if (row != null)
                {
                    state = new State();
                    int STATEID = offset + 0;
                    int STATENAME = offset + 1;
                    int COUNTRYID = offset + 2;

                    state.StateID = Convert.ToInt32(row[STATEID]);
                    state.StateName = Convert.ToString(row[STATENAME]);
                    state.CountryID = Convert.ToInt32(row[COUNTRYID]);
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
            return state;
        }

        /// <summary>
        /// Gets all <see cref="States"/> available in database.
        /// </summary>
        /// <returns>Object mapping of <see cref="States"/> in database.</returns>
        public static States GetStates()
        {
            States states = new States();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetStates";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        states.Add(FillState(row, 0));
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
            return states;
        }

        public static States GetStates(int countryID)
        {
            States states = new States();

            Object[] parameter = new object[1];
            parameter[0] = countryID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCountryStates", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        states.Add(FillState(row, 0));
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
            return states;
        }
        #endregion

        #region Flag

        /// <summary>
        /// Fills the row of flag to <see cref="Flag"/> object.
        /// </summary>
        /// <param name="row">Datarow to be filled.</param>
        /// <param name="offset">offset</param>
        /// <returns>Object of <see cref="Flag"/></returns>
        private static Flag FillFlag(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            Flag flag = null;
            try
            {
                if (row != null)
                {
                    flag = new Flag();
                    int COUNTRYFLAGID = offset + 0;
                    int COUNTRYFLAGNAME = offset + 1;
                    int COUNTRYFLAGIMAGE = offset + 2;

                    flag.CountryFlagID = Convert.ToInt32(row[COUNTRYFLAGID]);
                    flag.CountryFlagName = Convert.ToString(row[COUNTRYFLAGNAME]);
                    flag.CountryFlagImage = (byte[])(row[COUNTRYFLAGIMAGE]);
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
            return flag;
        }

        /// <summary>
        /// Gets all <see cref="Flags"/> available in database.
        /// </summary>
        /// <returns>Object mapping of <see cref="Flags"/> in database.</returns>
        public static Flags GetFlags()
        {
            Flags flags = new Flags();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllFlags";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        flags.Add(FillFlag(row, 0));
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
            return flags;
        }

        public static Flags GetFlags(int countryFlagID)
        {
            Flags flags = new Flags();

            Object[] parameter = new object[1];
            parameter[0] = countryFlagID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCountryFlag", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        flags.Add(FillFlag(row, 0));
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
            return flags;
        }

        public static int SaveCountryFlag(Flag flag)
        {
            int countryFlagID = int.MinValue;
            object[] parameter = new object[3];
            parameter[0] = flag.CountryFlagID;
            parameter[1] = flag.CountryFlagName;
            parameter[2] = flag.CountryFlagImage;

            try
            {
                countryFlagID = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCountryFlag", parameter).ToString());
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
            return countryFlagID;
        }

        #endregion

        #region Logo
        /// <summary>
        /// Fills the row of logo to <see cref="Logo"/> object.
        /// </summary>
        /// <param name="row">Datarow to be filled.</param>
        /// <param name="offset">offset</param>
        /// <returns>Object of <see cref="Logo"/></returns>
        private static Logo FillLogo(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            Logo logo = null;
            try
            {
                if (row != null)
                {
                    logo = new Logo();
                    int LOGOID = offset + 0;
                    int LOGONAME = offset + 1;
                    int LOGOIMAGE = offset + 2;

                    logo.LogoID = Convert.ToInt32(row[LOGOID]);
                    logo.LogoName = Convert.ToString(row[LOGONAME]);
                    logo.LogoImage = (byte[])(row[LOGOIMAGE]);
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
            return logo;
        }

        /// <summary>
        /// Gets all <see cref="Logos"/> available in database.
        /// </summary>
        /// <returns>Object mapping of <see cref="Logos"/> in database.</returns>
        public static Logos GetLogos()
        {
            Logos logos = new Logos();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllLogos";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        logos.Add(FillLogo(row, 0));
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
            return logos;
        }

        public static Logos GetLogos(int logoID)
        {
            Logos logos = new Logos();

            Object[] parameter = new object[1];
            parameter[0] = logoID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetLogo", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        logos.Add(FillLogo(row, 0));
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
            return logos;
        }

        public static int SaveLogo(Logo logo)
        {
            int logoID = int.MinValue;
            object[] parameter = new object[3];
            parameter[0] = logo.LogoID;
            parameter[1] = logo.LogoName;
            parameter[2] = logo.LogoImage;

            try
            {
                logoID = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveLogo", parameter).ToString());
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
            return logoID;
        }
        #endregion

        #region Holidays
        public static bool DeleteMasterHoliday(int holidayID)
        {
            bool result = false;

            try
            {
                object[] parameter = new object[1];
                parameter[0] = holidayID;
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

        #region Company_Logo
        /// <summary>
        /// Fills the row of logo to <see cref="Logo"/> object.
        /// </summary>
        /// <param name="row">Datarow to be filled.</param>
        /// <param name="offset">offset</param>
        /// <returns>Object of <see cref="Logo"/></returns>
        private static Logo FillCompanyLogo(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            Logo logo = null;
            try
            {
                if (row != null)
                {
                    logo = new Logo();
                    int LOGOID = offset + 0;
                    int LOGONAME = offset + 1;
                    int LOGOIMAGE = offset + 2;

                    logo.LogoID = Convert.ToInt32(row[LOGOID]);
                    logo.LogoName = Convert.ToString(row[LOGONAME]);
                    logo.LogoImage = (byte[])(row[LOGOIMAGE]);
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
            return logo;
        }

        public static Logo GetCompanyLogo()
        {
            Logo companyLogo = new Logo();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetCompanyLogo";

            //Object[] parameter = new object[1];
            //parameter[0] = companyID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyLogo = FillCompanyLogo(row, 0);
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
            return companyLogo;
        }

        /// <summary>
        /// Gets all <see cref="Logos"/> available in database.
        /// </summary>
        /// <returns>Object mapping of <see cref="Logos"/> in database.</returns>
        public static Logos GetCompanyLogos()
        {
            Logos companyLogos = new Logos();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllLogosForCompany";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyLogos.Add(FillCompanyLogo(row, 0));
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
            return companyLogos;
        }
        public static Logos GetPranaLogo()
        {
            Logos companyLogos = new Logos();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetPranaLogo";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyLogos.Add(FillCompanyLogo(row, 0));
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
            return companyLogos;
        }


        public static int SaveCompanyLogo(Logo logo)
        {
            int logoID = int.MinValue;
            object[] parameter = new object[3];
            parameter[0] = logo.LogoID;
            parameter[1] = logo.LogoName;
            parameter[2] = logo.LogoImage;

            try
            {
                logoID = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyLogo", parameter).ToString());
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
            return logoID;
        }

        public static int SavePranaLogo(Logo logo)
        {
            int logoID = int.MinValue;
            object[] parameter = new object[3];
            parameter[0] = logo.LogoID;
            parameter[1] = logo.LogoName;
            parameter[2] = logo.LogoImage;

            try
            {
                logoID = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SavePranaLogo", parameter).ToString());
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
            return logoID;
        }
        #endregion

    }
}
