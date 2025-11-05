using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// CounterPartyManager would be a static class to handel <see cref="CounterParty"/> related details.
    /// </summary>
    /// <remarks>Its a Static class like other managers class so that we don't have to instatiate it again and again while working with them.</remarks>
    public class CounterPartyManager
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private CounterPartyManager()
        {
        }

        #endregion

        #region Basic methods like Add/Update/Delete/Get

        public static CounterParty FillCounterParties(object[] row, int offSet)
        {
            int counterpartyID = 0 + offSet;
            int fullName = 1 + offSet;
            int shortName = 2 + offSet;
            int address = 3 + offSet;
            int phone = 4 + offSet;
            int fax = 5 + offSet;
            int contactname1 = 6 + offSet;
            int title1 = 7 + offSet;
            int email1 = 8 + offSet;
            int contactname2 = 9 + offSet;
            int title2 = 10 + offSet;
            int email2 = 11 + offSet;
            int counterpartytypeid = 12 + offSet;

            int address2 = 13 + offSet;
            int countryID = 14 + offSet;
            int stateID = 15 + offSet;
            int zip = 16 + offSet;
            int contactName1LastName = 17 + offSet;
            int contactName1WorkPhone = 18 + offSet;
            int contactName1CellPhone = 19 + offSet;
            int contactName2LastName = 20 + offSet;
            int contactName2WorkPhone = 21 + offSet;
            int contactName2CellPhone = 22 + offSet;
            int city = 23 + offSet;
            int isAlgoBroker = 24 + offSet;
            int isOTDorEMS = 25 + offSet;

            CounterParty counterParty = new CounterParty();
            try
            {
                if (row[counterpartyID] != System.DBNull.Value)
                {
                    counterParty.CounterPartyID = int.Parse(row[counterpartyID].ToString());
                }
                if (row[fullName] != System.DBNull.Value)
                {
                    counterParty.CounterPartyFullName = row[fullName].ToString();
                }
                if (row[shortName] != System.DBNull.Value)
                {
                    counterParty.ShortName = row[shortName].ToString();
                }
                if (row[address] != System.DBNull.Value)
                {
                    counterParty.Address = row[address].ToString();
                }
                if (row[phone] != System.DBNull.Value)
                {
                    counterParty.Phone = row[phone].ToString();
                }
                if (row[fax] != System.DBNull.Value)
                {
                    counterParty.Fax = row[fax].ToString();
                }
                if (row[contactname1] != System.DBNull.Value)
                {
                    counterParty.ContactName1 = row[contactname1].ToString();
                }
                if (row[title1] != System.DBNull.Value)
                {
                    counterParty.Title1 = row[title1].ToString();
                }
                if (row[email1] != System.DBNull.Value)
                {
                    counterParty.Email1 = row[email1].ToString();
                }
                if (row[contactname2] != System.DBNull.Value)
                {
                    counterParty.contactName2 = row[contactname2].ToString();
                }
                if (row[title2] != System.DBNull.Value)
                {
                    counterParty.Title2 = row[title2].ToString();
                }
                if (row[email2] != System.DBNull.Value)
                {
                    counterParty.Email2 = row[email2].ToString();
                }
                if (row[counterpartytypeid] != System.DBNull.Value)
                {
                    counterParty.CounterPartyTypeID = int.Parse(row[counterpartytypeid].ToString());
                }

                if (row[address2] != System.DBNull.Value)
                {
                    counterParty.Address2 = row[address2].ToString();
                }
                if (row[countryID] != System.DBNull.Value)
                {
                    counterParty.CountryID = int.Parse(row[countryID].ToString());
                }
                if (row[stateID] != System.DBNull.Value)
                {
                    counterParty.StateID = int.Parse(row[stateID].ToString());
                }
                if (row[zip] != System.DBNull.Value)
                {
                    counterParty.Zip = row[zip].ToString();
                }
                if (row[contactName1LastName] != System.DBNull.Value)
                {
                    counterParty.ContactName1LastName = row[contactName1LastName].ToString();
                }
                if (row[contactName1WorkPhone] != System.DBNull.Value)
                {
                    counterParty.ContactName1WorkPhone = row[contactName1WorkPhone].ToString();
                }
                if (row[contactName1CellPhone] != System.DBNull.Value)
                {
                    counterParty.ContactName1CellPhone = row[contactName1CellPhone].ToString();
                }
                if (row[contactName2LastName] != System.DBNull.Value)
                {
                    counterParty.ContactName2LastName = row[contactName2LastName].ToString();
                }
                if (row[contactName2WorkPhone] != System.DBNull.Value)
                {
                    counterParty.ContactName2WorkPhone = row[contactName2WorkPhone].ToString();
                }
                if (row[contactName2CellPhone] != System.DBNull.Value)
                {
                    counterParty.ContactName2CellPhone = row[contactName2CellPhone].ToString();
                }
                if (row[city] != System.DBNull.Value)
                {
                    counterParty.City = row[city].ToString();
                }
                if (row[isAlgoBroker] != System.DBNull.Value)
                {
                    counterParty.IsAlgoBroker = Convert.ToBoolean(row[isAlgoBroker].ToString());
                }
                if (row[isOTDorEMS] != System.DBNull.Value)
                {
                    counterParty.IsOTDorEMS = Convert.ToBoolean(row[isOTDorEMS].ToString());
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
            return counterParty;
        }

        /// <summary>
        /// Gets <see cref="CounterParty"/> from the database.
        /// </summary>
        /// <param name="counterPartyID">ID of <see cref="CounterParty"/> to be fetched from database.</param>
        /// <returns><see cref="CounterParty"/> fetched.</returns>
        public static CounterParties GetCounterParties()
        {
            CounterParties counterParties = new CounterParties();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllCounterParties";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        counterParties.Add(FillCounterParties(row, 0));
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
            return counterParties;
        }

        /// <summary>
        /// Gets all <see cref="CounterParty"/> from datatbase.
        /// </summary>
        /// <returns><see cref="CounterParty"/> fetched.</returns>
        public static CounterParty GetCounterParty(int counterPartyID)
        {
            CounterParty counterParty = new CounterParty();

            object[] parameter = new object[1];
            parameter[0] = counterPartyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCounterParty", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        counterParty = FillCounterParties(row, 0);
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
            return counterParty;
        }

        public static CounterPartyVenues GetCounterPartyVenuesForUser(int companyUserID)
        {
            CounterPartyVenues counterPartyVenues = new CounterPartyVenues();

            object[] parameter = new object[1];
            parameter[0] = companyUserID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCounterPartyVenuesForUser", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        counterPartyVenues.Add(FillCounterPartyVenues(row, 0));
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
            return counterPartyVenues;
        }

        /// <summary>
        /// Deletes <see cref="CounterParty"/> from the database.
        /// </summary>
        /// <param name="counterPartyID">ID of the <see cref="CounterParty"/> to be deleted.</param>
        /// <returns>Result. True if succesfull else False.</returns>
        public static bool DeleteCounterParty(int counterPartyID)
        {
            bool result = false;
            object[] parameter = new object[1];
            parameter[0] = counterPartyID;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCounterParty", parameter) > 0)
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
            return result;
        }

        /// <summary>
        /// Adds/Updates <see cref="CounterParty"/> into database.
        /// </summary>
        /// <param name="counterParty"><see cref="CounterParty"/> to be added.</param>
        /// <returns>Result. True if succesfull else False.</returns>
        public static int SaveCounterParty(CounterParty counterParty)
        {
            int result = int.MinValue;

            object[] parameter = new object[27];

            parameter[0] = counterParty.CounterPartyID;
            parameter[1] = counterParty.CounterPartyFullName;
            parameter[2] = counterParty.ShortName;
            parameter[3] = counterParty.Address;
            parameter[4] = counterParty.Phone;
            parameter[5] = counterParty.Fax;
            parameter[6] = counterParty.ContactName1;
            parameter[7] = counterParty.Title1;
            parameter[8] = counterParty.Email1;
            parameter[9] = counterParty.contactName2;
            parameter[10] = counterParty.Title2;
            parameter[11] = counterParty.Email2;
            parameter[12] = counterParty.CounterPartyTypeID;

            parameter[13] = counterParty.Address2;
            parameter[14] = counterParty.CountryID;
            parameter[15] = counterParty.StateID;
            parameter[16] = counterParty.Zip;
            parameter[17] = counterParty.ContactName1LastName;
            parameter[18] = counterParty.ContactName1WorkPhone;
            parameter[19] = counterParty.ContactName1CellPhone;
            parameter[20] = counterParty.ContactName2LastName;
            parameter[21] = counterParty.ContactName2WorkPhone;
            parameter[22] = counterParty.ContactName2CellPhone;

            parameter[23] = counterParty.City;
            parameter[24] = counterParty.IsAlgoBroker;
            parameter[25] = int.MinValue;
            parameter[26] = counterParty.IsOTDorEMS;

            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCounterParty", parameter).ToString());
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

        #region CompanyCounterParties

        public static CounterParty FillCompanyCounterParties(object[] row, int offSet)
        {
            int companyID = 0 + offSet;
            int counterPartyID = 1 + offSet;

            int fullName = 2 + offSet;
            int shortName = 3 + offSet;
            int address = 4 + offSet;
            int phone = 5 + offSet;
            int fax = 6 + offSet;
            int contactname1 = 7 + offSet;
            int title1 = 8 + offSet;
            int email1 = 9 + offSet;
            int contactname2 = 10 + offSet;
            int title2 = 11 + offSet;
            int email2 = 12 + offSet;
            int counterpartytypeid = 13 + offSet;
            int isOTDorEMS = 14 + offSet;


            CounterParty counterParty = new CounterParty();
            try
            {
                if (row[companyID] != null)
                {
                    counterParty.CompanyID = int.Parse(row[companyID].ToString());
                }
                if (row[counterPartyID] != null)
                {
                    counterParty.CounterPartyID = int.Parse(row[counterPartyID].ToString());
                }
                if (row[fullName] != null)
                {
                    counterParty.CounterPartyFullName = row[fullName].ToString();
                }
                if (row[shortName] != null)
                {
                    counterParty.ShortName = row[shortName].ToString();
                }
                if (row[address] != null)
                {
                    counterParty.Address = row[address].ToString();
                }
                if (row[phone] != null)
                {
                    counterParty.Phone = row[phone].ToString();
                }
                if (row[fax] != null)
                {
                    counterParty.Fax = row[fax].ToString();
                }
                if (row[contactname1] != null)
                {
                    counterParty.ContactName1 = row[contactname1].ToString();
                }
                if (row[title1] != null)
                {
                    counterParty.Title1 = row[title1].ToString();
                }
                if (row[email1] != null)
                {
                    counterParty.Email1 = row[email1].ToString();
                }
                if (row[contactname2] != null)
                {
                    counterParty.contactName2 = row[contactname2].ToString();
                }
                if (row[title2] != null)
                {
                    counterParty.Title2 = row[title2].ToString();
                }
                if (row[email2] != null)
                {
                    counterParty.Email2 = row[email2].ToString();
                }
                if (row[counterpartytypeid] != null)
                {
                    counterParty.CounterPartyTypeID = int.Parse(row[counterpartytypeid].ToString());
                }
                if (row[isOTDorEMS] != null)
                {
                    counterParty.IsOTDorEMS = Convert.ToBoolean(row[isOTDorEMS].ToString());
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
            return counterParty;
        }

        public static CounterParty FillCounterPartiesForTradingTicket(object[] row, int offSet)
        {

            int counterPartyID = 0 + offSet;

            int fullName = 1 + offSet;
            int shortName = 2 + offSet;



            CounterParty counterParty = new CounterParty();
            try
            {

                if (row[counterPartyID] != null)
                {
                    counterParty.CounterPartyID = int.Parse(row[counterPartyID].ToString());
                }
                if (row[fullName] != null)
                {
                    counterParty.CounterPartyFullName = row[fullName].ToString();
                }
                if (row[shortName] != null)
                {
                    counterParty.ShortName = row[shortName].ToString();
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
            return counterParty;
        }

        public static bool SaveCompanyCounterPartyVenues(int companyID, CounterPartyVenues counterPartyVenues)
        {
            object[] parameter = new object[1];
            parameter[0] = companyID;
            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyCounterPartyVenues", parameter).ToString();

                foreach (CounterPartyVenue counterPartyVenue in counterPartyVenues)
                {
                    parameter = new object[2];
                    parameter[0] = companyID;
                    parameter[1] = counterPartyVenue.CounterPartyVenueID;


                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveCompanyCounterPartyVenues", parameter).ToString();
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
            return true;
        }

        public static bool SaveCompanyCounterParties(int companyID, CounterParties counterParties)
        {
            object[] parameter = new object[1];
            parameter[0] = companyID;
            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyCounterParties", parameter).ToString();

                foreach (CounterParty counterParty in counterParties)
                {
                    parameter = new object[2];
                    parameter[0] = companyID;
                    parameter[1] = counterParty.CounterPartyID;


                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveCompanyCounterParties", parameter).ToString();
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
            return true;
        }

        public static bool SaveCompanyUserCounterParties(int companyID, CounterParties counterParties, int userID)
        {
            object[] parameter = new object[2];
            parameter[0] = companyID;
            parameter[1] = userID;
            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyUserCounterPartiesNew", parameter).ToString();

                foreach (CounterParty counterParty in counterParties)
                {
                    parameter = new object[3];
                    parameter[0] = companyID;
                    parameter[1] = counterParty.CounterPartyID;
                    parameter[2] = userID;

                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveCompanyUserCounterParties", parameter).ToString();
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
            return true;
        }

        public static bool SaveCounterPartyVenuesForUser(int companyID, int userID, CounterPartyVenues counterPartyVenues)
        {
            StringBuilder ccpvIDStringBuilder = new StringBuilder();
            int result = int.MinValue;

            object[] parameter = new object[1];
            parameter[0] = userID;
            try
            {
                //db.ExecuteNonQuery("P_DeleteCounterPartyVenuesForUser", parameter).ToString();

                foreach (CounterPartyVenue counterPartyVenue in counterPartyVenues)
                {
                    parameter = new object[4];
                    parameter[0] = counterPartyVenue.CounterPartyID;
                    parameter[1] = counterPartyVenue.VenueID;
                    parameter[2] = userID;
                    parameter[3] = companyID;

                    result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCounterPartyVenuesForUser", parameter).ToString());

                    ccpvIDStringBuilder.Append("'");
                    ccpvIDStringBuilder.Append(result.ToString());
                    ccpvIDStringBuilder.Append("',");
                }

                int len = ccpvIDStringBuilder.Length;
                if (ccpvIDStringBuilder.Length > 0)
                {
                    ccpvIDStringBuilder.Remove((len - 1), 1);
                }
                parameter = new object[2];

                parameter[0] = userID;
                parameter[1] = ccpvIDStringBuilder.ToString();

                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCounterPartyVenuesForUser", parameter).ToString();

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
            return true;
        }

        public static bool CheckExistingUserCounterPartyVenue(int counterPartyID, int venueID, int companyID)
        {
            int result = 0;
            object[] parameter = new object[3];
            try
            {
                parameter = new object[3];
                parameter[0] = counterPartyID;
                parameter[1] = venueID;
                parameter[2] = companyID;

                //result = db.ExecuteNonQuery("P_CheckExistingUserCounterPartyVenue", parameter);
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_CheckExistingUserCounterPartyVenue", parameter).ToString());

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
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static CounterParties GetCompanyCounterParties(int companyID)
        {
            CounterParties companyCounterParties = new CounterParties();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyCounterParties", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyCounterParties.Add(FillCompanyCounterParties(row, 0));

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
            return companyCounterParties;
        }

        public static CounterParties GetCompanyUserCounterParties(int companyID, int userID)
        {
            CounterParties companyCounterParties = new CounterParties();

            object[] parameter = new object[2];
            parameter[0] = companyID;
            parameter[1] = userID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyUserCounterParties", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyCounterParties.Add(FillCompanyCounterParties(row, 0));

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
            return companyCounterParties;
        }

        public static CounterPartyVenues GetCompanyCounterPartyVeneus(int companyID)
        {
            CounterPartyVenues counterPartyVenues = new CounterPartyVenues();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyCounterPartyVenues", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        counterPartyVenues.Add(FillCounterPartyVenues(row, 0));

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
            return counterPartyVenues;
        }

        public static CounterParties GetCounterPartiesByAUIDAndUserID(int userID, int AssetID, int UnderlyingID)
        {
            CounterParties companyCounterParties = new CounterParties();

            object[] parameter = new object[3];
            parameter[0] = userID;
            parameter[1] = AssetID;
            parameter[2] = UnderlyingID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCounterPartiesByAUIDAndUserID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyCounterParties.Add(FillCounterPartiesForTradingTicket(row, 0));

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
            return companyCounterParties;
        }
        public static CounterParties GetCompanyUserCounterParties(int userID)
        {
            CounterParties companyCounterParties = new CounterParties();

            object[] parameter = new object[1];

            parameter[0] = userID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCounterPartiesByUserID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyCounterParties.Add(FillCounterPartiesForTradingTicket(row, 0));

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
            return companyCounterParties;
        }
        #endregion

        #region CompanyCounterPartyVenues With Changes dated 06-01-2006
        public static CompanyCounterPartyVenue FillCompanyCounterPartyVenue(object[] row, int offSet)
        {
            int CompanyCounterPartyCVID = 0 + offSet;
            int CompanyID = 1 + offSet;
            int CounterPartyVenueID = 2 + offSet;
            int CounterPartyVenueDisplayName = 3 + offSet;

            CompanyCounterPartyVenue companyCounterPartyVenue = new CompanyCounterPartyVenue();
            try
            {

                if (!(row[CompanyCounterPartyCVID] is System.DBNull))
                {
                    companyCounterPartyVenue.CompanyCounterPartyCVID = int.Parse(row[CompanyCounterPartyCVID].ToString());
                }
                if (!(row[CompanyID] is System.DBNull))
                {
                    companyCounterPartyVenue.CompanyID = int.Parse(row[CompanyID].ToString());
                }
                if (!(row[CounterPartyVenueID] is System.DBNull))
                {
                    companyCounterPartyVenue.CounterPartyVenueID = int.Parse(row[CounterPartyVenueID].ToString());
                }
                if (!(row[CounterPartyVenueDisplayName] is System.DBNull))
                {
                    companyCounterPartyVenue.CounterPartyVenueDisplayName = row[CounterPartyVenueDisplayName].ToString();
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
            return companyCounterPartyVenue;
        }

        public static CompanyCounterPartyVenues GetCompanyCounterPartyVeneusChanged(int companyID, int companyCounterPartyID)
        {
            CompanyCounterPartyVenues companyCounterPartyVenues = new CompanyCounterPartyVenues();

            object[] parameter = new object[2];
            parameter[0] = companyID;
            parameter[1] = companyCounterPartyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyCounterPartyVenuesChanged", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyCounterPartyVenues.Add(FillCompanyCounterPartyVenue(row, 0));

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
            return companyCounterPartyVenues;
        }

        public static CompanyCounterPartyVenue GetCompanyCounterPartyVenue(int companyCounterPartyVenueID)
        {
            CompanyCounterPartyVenue companyCounterPartyVenue = new CompanyCounterPartyVenue();

            object[] parameter = new object[1];
            parameter[0] = companyCounterPartyVenueID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyCounterPartyVenue", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyCounterPartyVenue = FillCompanyCounterPartyVenue(row, 0);

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
            return companyCounterPartyVenue;
        }

        /// <summary>
        /// Created by : Kanupriya
        /// Purpose: To fetch the CompanyCounterPartyVenueID for a particular combination of CVID and CompanyID.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p_2"></param>
        /// <returns></returns>
        public static CompanyCounterPartyVenue GetCompanyCounterPartyVenueDetails(int counterPartyID, int venueID, int companyID)
        {
            CompanyCounterPartyVenue companyCounterPartyVenue = new CompanyCounterPartyVenue();

            object[] parameter = new object[3];
            parameter[0] = counterPartyID;
            parameter[1] = venueID;
            parameter[2] = companyID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCounterPartyVenueDetail", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyCounterPartyVenue = FillCompanyCounterPartyVenueDetail(row, 0);
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
            return companyCounterPartyVenue;
        }

        /// <summary>
        /// Created by :<Kanupriya>
        /// The method is used to fill a row of the object CompanyCounterPartyVenue .
        /// </summary>
        /// <param name="row"></param>
        /// <param name="offSet"></param>
        /// <returns></returns>
        private static CompanyCounterPartyVenue FillCompanyCounterPartyVenueDetail(object[] row, int offSet)
        {
            int counterPartyVenueID = offSet + 0;
            int companyCVID = offSet + 1;

            CompanyCounterPartyVenue companyCounterPartyVenue = new CompanyCounterPartyVenue();
            try
            {
                if (!(row[counterPartyVenueID] is System.DBNull))
                {
                    companyCounterPartyVenue.CounterPartyVenueID = int.Parse(row[counterPartyVenueID].ToString());
                }
                if (!(row[companyCVID] is System.DBNull))
                {
                    companyCounterPartyVenue.CompanyCounterPartyCVID = int.Parse(row[companyCVID].ToString());
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
            return companyCounterPartyVenue;

        }

        #endregion

        #region CounterParty Type
        /// <summary>
        /// Fills <see cref="CounterParty"/>.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="offSet"></param>
        /// <returns></returns>
        private static CounterPartyType FillCounterPartyTypes(object[] row, int offSet)
        {
            int ID = 0 + offSet;
            int type = 1 + offSet;

            CounterPartyType counterPartyType = new CounterPartyType();
            try
            {
                if (row[ID] != null)
                {
                    counterPartyType.CounterPartyTypeID = int.Parse(row[ID].ToString());
                }
                if (row[type] != null)
                {
                    counterPartyType.Type = row[type].ToString();
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
            return counterPartyType;
        }

        /// <summary>
        /// Gets all <see cref="CounterPartyTypes"/> from database.
        /// </summary>
        /// <returns><see cref="CounterPartyTypes"/> fetched.</returns>
        public static CounterPartyTypes GetCounterPartyTypes()
        {
            CounterPartyTypes counterPartyTypes = new CounterPartyTypes();

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetCounterPartyTypes";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        counterPartyTypes.Add(FillCounterPartyTypes(row, 0));
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
            return counterPartyTypes;
        }

        public static int SaveCounterPartyType(CounterPartyType counterPartyType)
        {
            int result = int.MinValue;

            object[] parameter = new object[3];

            parameter[0] = counterPartyType.CounterPartyTypeID;
            parameter[1] = counterPartyType.Type;
            parameter[2] = int.MinValue;
            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCounterPartyType", parameter).ToString());
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

        public static bool DeleteCounterPartyType(int counterPartyTypeID)
        {
            bool result = false;
            try
            {
                object[] parameter = new object[1];
                parameter[0] = counterPartyTypeID;
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCounterPartyType", parameter) > 0)
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

        #region CounterPartyVenueDetails

        //TODO: Remove the unused fields from here.
        public static CounterPartyVenue FillCounterPartyVenues(object[] row, int offSet)
        {
            int counterPartyVenueID = 0 + offSet;
            int displayName = 1 + offSet;
            int isElectronic = 2 + offSet;
            int oatsIdentifier = 3 + offSet;
            int symbolConventionID = 4 + offSet;
            int counterPartyID = 5 + offSet;
            int venueID = 6 + offSet;
            int currencyID = 7 + offSet;
            int companyCounterPartyCVID = 8 + offSet;


            CounterPartyVenue counterPartyVenue = new CounterPartyVenue();
            try
            {

                if (!(row[counterPartyVenueID] is System.DBNull))
                {
                    counterPartyVenue.CounterPartyVenueID = int.Parse(row[counterPartyVenueID].ToString());
                }
                if (!(row[displayName] is System.DBNull))
                {
                    counterPartyVenue.DisplayName = row[displayName].ToString();
                }
                if (!(row[isElectronic] is System.DBNull))
                {
                    counterPartyVenue.IsElectronic = int.Parse(row[isElectronic].ToString());
                }
                if (!(row[oatsIdentifier] is System.DBNull))
                {
                    counterPartyVenue.OatsIdentifier = row[oatsIdentifier].ToString();
                }
                if (!(row[symbolConventionID] is System.DBNull))
                {
                    counterPartyVenue.SymbolConventionID = int.Parse(row[symbolConventionID].ToString());
                }

                counterPartyVenue.CounterPartyID = int.Parse(row[counterPartyID].ToString());
                counterPartyVenue.VenueID = int.Parse(row[venueID].ToString());

                if (!(row[currencyID] is System.DBNull))
                {
                    counterPartyVenue.BaseCurrencyID = int.Parse(row[currencyID].ToString());
                }

                if (!(row[companyCounterPartyCVID] is System.DBNull))
                {
                    counterPartyVenue.CompanyCounterPartyCVID = int.Parse(row[companyCounterPartyCVID].ToString());
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
            return counterPartyVenue;
        }

        public static CounterPartyVenues GetCounterPartyVenues()
        {
            CounterPartyVenues counterPartyVenues = new CounterPartyVenues();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllCounterPartyVenues";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        counterPartyVenues.Add(FillCounterPartyVenues(row, 0));
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
            return counterPartyVenues;
        }

        public static CounterPartyVenue GetCounterPartyVenue(int counterPartyVenueID)
        {
            CounterPartyVenue counterPartyVenue = new CounterPartyVenue();

            object[] parameter = new object[1];
            parameter[0] = counterPartyVenueID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCounterPartyVenue", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        counterPartyVenue = FillCounterPartyVenues(row, 0);
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
            return counterPartyVenue;
        }


        public static bool DeleteCounterPartyVenue(int counterPartyVenueID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            parameter[0] = counterPartyVenueID;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCounterPartyVenue", parameter) > 0)
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
            return result;
        }

        public static int SaveCounterPartyVenue(CounterPartyVenue counterPartyVenue)
        {
            int result = int.MinValue;

            object[] parameter = new object[9];

            parameter[0] = counterPartyVenue.CounterPartyVenueID;
            parameter[1] = counterPartyVenue.DisplayName;
            parameter[2] = counterPartyVenue.IsElectronic;
            parameter[3] = counterPartyVenue.OatsIdentifier;
            parameter[4] = counterPartyVenue.SymbolConventionID;
            parameter[5] = counterPartyVenue.BaseCurrencyID;
            parameter[6] = counterPartyVenue.CounterPartyID;
            parameter[7] = counterPartyVenue.VenueID;

            parameter[8] = int.MinValue;



            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCounterPartyVenue", parameter).ToString());
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

        public static int SaveCounterPartyVenueDialog(CounterPartyVenue counterPartyVenue)
        {
            int result = int.MinValue;

            object[] parameter = new object[4];

            parameter[0] = counterPartyVenue.CounterPartyVenueID;
            parameter[1] = counterPartyVenue.CounterPartyID;
            parameter[2] = counterPartyVenue.VenueID;
            parameter[3] = int.MinValue;

            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCounterPartyVenueDialog", parameter).ToString());
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



        public static Side FillCVAUECSide(object[] row, int offSet)
        {
            int CVAUECID = 0 + offSet;
            int SIDEID = 1 + offSet;

            Side side = new Side();
            try
            {
                if (row[CVAUECID] != null)
                {
                    side.CVAUECID = int.Parse(row[CVAUECID].ToString());
                }
                if (row[SIDEID] != null)
                {
                    side.SideID = int.Parse(row[SIDEID].ToString());
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
            return side;
        }

        /// <summary>
        /// This method fetches the sides corresponding to the particular cvAuecID.
        /// </summary>
        /// <param name="cvAuecID">The parameter against which the sides are fetched.</param>
        /// <returns>Collecion of <see cref="Side"/> in <see cref="Sides"/></returns>
        public static Sides GetCVAUECSides(int cvAuecID)
        {
            Sides sides = new Sides();

            object[] parameter = new object[1];
            parameter[0] = cvAuecID;
            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCVAUECSides", parameter))
            {
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    sides.Add(FillCVAUECSide(row, 0));
                }
            }
            return sides;
        }

        public static bool SaveCVAUECSide(int cvAuecID, Sides sides)
        {
            bool result = false;

            object[] parameter = new object[1];
            parameter[0] = cvAuecID;

            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCVAUECSides", parameter).ToString();

                foreach (Side side in sides)
                {
                    parameter = new object[2];
                    parameter[0] = cvAuecID;
                    parameter[1] = side.SideID;
                    if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveCVAUECSide", parameter) > 0)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
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
            return result;
        }



        #region CVAUECOrderTypes
        public static OrderType FillCVAUECOrderType(object[] row, int offSet)
        {
            int CVAUECID = 0 + offSet;
            int ORDERTYPEID = 1 + offSet;

            OrderType orderType = new OrderType();
            try
            {
                if (row[CVAUECID] != null)
                {
                    orderType.CVAUECID = int.Parse(row[CVAUECID].ToString());
                }
                if (row[ORDERTYPEID] != null)
                {
                    orderType.OrderTypesID = int.Parse(row[ORDERTYPEID].ToString());
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
            return orderType;
        }

        /// <summary>
        /// This method fetches the orderTypes corresponding to the particular cvAuecID.
        /// </summary>
        /// <param name="cvAuecID">The parameter against which the sides are fetched.</param>
        /// <returns>Collecion of <see cref="OrderType"/> in <see cref="OrderTypes"/></returns>
        public static OrderTypes GetCVAUECOrderTypes(int cvAuecID)
        {
            OrderTypes orderTypes = new OrderTypes();

            object[] parameter = new object[1];
            parameter[0] = cvAuecID;

            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCVAUECOrderTypes", parameter))
            {
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    orderTypes.Add(FillCVAUECOrderType(row, 0));
                }
            }
            return orderTypes;
        }
        #endregion

        #region CVAUEC
        /// <summary>
        /// Fills the details corresponding to the particular AUEC. 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="offSet"></param>
        /// <returns></returns>
        public static AUEC FillCVAUEC(object[] row, int offSet)
        {
            //int COUNTERPARTYVENUEID = 0 + offSet;
            int AUECID = 0 + offSet;
            //These all are required to make complete definition for AUEC i.e. to show the relative names of
            //Asset, Underlying & exchange.
            int ASSETID = 1 + offSet;
            int UNDERLYINGID = 2 + offSet;
            int EXCHANGEID = 3 + offSet;
            int CURRENCYID = 4 + offSet;
            int DISPLAYNAME = 5 + offSet;

            AUEC aUEC = new AUEC();
            try
            {
                if (row[AUECID] != null)
                {
                    aUEC.AUECID = int.Parse(row[AUECID].ToString());
                }
                if (row[ASSETID] != null)
                {
                    aUEC.AssetID = int.Parse(row[ASSETID].ToString());
                }
                if (row[UNDERLYINGID] != null)
                {
                    aUEC.UnderlyingID = int.Parse(row[UNDERLYINGID].ToString());
                }
                if (row[EXCHANGEID] != null)
                {
                    aUEC.ExchangeID = int.Parse(row[EXCHANGEID].ToString());
                }
                if (row[CURRENCYID] != null)
                {
                    aUEC.CurrencyID = int.Parse(row[CURRENCYID].ToString());
                }
                if (row[DISPLAYNAME] != null)
                {
                    aUEC.DisplayName = row[DISPLAYNAME].ToString();
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
            return aUEC;
        }

        /// <summary>
        /// Fills the CounterParty Venue, CVAUECID & AUECID.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="offSet"></param>
        /// <returns>The counterpartyvenue object after filling the required details.</returns>
        public static CounterPartyVenue FillCVAUECDetails(object[] row, int offSet)
        {
            int CVAUECID = 0 + offSet;
            int COUNTERPARTYVENUEID = 1 + offSet;
            int AUECID = 2 + offSet;

            CounterPartyVenue counterPartyVenue = new CounterPartyVenue();
            try
            {
                if (row[CVAUECID] != null)
                {
                    counterPartyVenue.CVAUECID = int.Parse(row[CVAUECID].ToString());
                }
                if (row[COUNTERPARTYVENUEID] != null)
                {
                    counterPartyVenue.CounterPartyVenueID = int.Parse(row[COUNTERPARTYVENUEID].ToString());
                }
                if (row[AUECID] != null)
                {
                    counterPartyVenue.AUECID = int.Parse(row[AUECID].ToString());
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
            return counterPartyVenue;
        }

        #region CVAUECThirdParty
        /// <summary>
        /// This is a one off method made to accomodate the CVAUEC & AUEC related details taken together.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="offSet"></param>
        /// <returns>Object of type <see cref="AUEC"/>Object type AUEC</returns>
        public static AUEC FillCVAUECForThirdParty(object[] row, int offSet)
        {
            int CVAUECID = 0 + offSet;
            int COUNTERPARTYVENUEID = 1 + offSet;
            int AUECID = 2 + offSet;
            int ASSETID = 3 + offSet;
            int UNDERLYINGID = 4 + offSet;
            int EXCHANGEID = 5 + offSet;
            int CURRENCYID = 6 + offSet;
            int DISPLAYNAME = 7 + offSet;

            AUEC cvAUEC = new AUEC();
            try
            {
                if (row[CVAUECID] != null)
                {
                    cvAUEC.CVAUECID = int.Parse(row[CVAUECID].ToString());
                }
                if (row[COUNTERPARTYVENUEID] != null)
                {
                    cvAUEC.CounterPartyVenueID = int.Parse(row[COUNTERPARTYVENUEID].ToString());
                }
                if (row[AUECID] != null)
                {
                    cvAUEC.AUECID = int.Parse(row[AUECID].ToString());
                }
                if (row[ASSETID] != null)
                {
                    cvAUEC.AssetID = int.Parse(row[ASSETID].ToString());
                }
                if (row[UNDERLYINGID] != null)
                {
                    cvAUEC.UnderlyingID = int.Parse(row[UNDERLYINGID].ToString());
                }
                if (row[EXCHANGEID] != null)
                {
                    cvAUEC.ExchangeID = int.Parse(row[EXCHANGEID].ToString());
                }
                if (row[CURRENCYID] != null)
                {
                    cvAUEC.CurrencyID = int.Parse(row[CURRENCYID].ToString());
                }
                if (row[DISPLAYNAME] != null)
                {
                    cvAUEC.DisplayName = row[DISPLAYNAME].ToString();
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
            return cvAUEC;
        }


        /// <summary>
        /// This method fetches all the present CVAUECs in the table.
        /// </summary>
        /// <returns>Collection of CVAUEC</returns>
        public static AUECs GetAllCVAUECs()
        {
            AUECs aUECs = new AUECs();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllCVAUECs";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        aUECs.Add(FillCVAUECForThirdParty(row, 0));
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
            return aUECs;
        }

        /// <summary>
        /// This is a special method which gets the common AUEC's present for a particular CV and for a particular company.
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="companyCounterPartyCVID"></param>
        /// <returns>The collection of <see cref="AUEC"/> objects.</returns>
        public static AUECs GetAllCompanyCVAUECs(int companyID, int companyCounterPartyCVID)
        {
            AUECs aUECs = new AUECs();

            object[] parameter = new object[2];
            parameter[0] = companyID;
            parameter[1] = companyCounterPartyCVID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyCVAUECs", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        aUECs.Add(FillCVAUECForThirdParty(row, 0));
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
            return aUECs;
        }



        /// <summary>
        /// Get All Company CVAUECs StringName
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static Dictionary<int, List<CVAuecs>> GetAllCompanyCVAUECsStringName(int companyID)
        {
            Dictionary<int, List<CVAuecs>> CVAUECStringNmaes = new Dictionary<int, List<CVAuecs>>();

            object[] parameter = new object[1];
            parameter[0] = companyID;
            CVAuecs cvAuec = new CVAuecs();
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAllCompanyCVAUECsStringName", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        int counterPartyVenueID = Convert.ToInt32(row[0]);
                        cvAuec = new CVAuecs();
                        cvAuec.AuecID = Convert.ToInt32(row[1]);
                        cvAuec.AuecStringName = row[2].ToString();

                        if (CVAUECStringNmaes.ContainsKey(counterPartyVenueID))
                        {
                            CVAUECStringNmaes[counterPartyVenueID].Add(cvAuec);
                        }
                        else
                        {
                            List<CVAuecs> lstCVAuecs = new List<CVAuecs>();
                            lstCVAuecs.Add(cvAuec);
                            CVAUECStringNmaes.Add(counterPartyVenueID, lstCVAuecs);
                        }

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
            return CVAUECStringNmaes;
        }
        /// <summary>
        /// This method gets the CV AUEC's present for a particular given list of assets.
        /// </summary>
        /// <param name="companyCounterPartyCVID"></param>
        /// <returns>The collection of <see cref="AUEC"/> objects.</returns>
        public static List<int> GetAllCVAUECsForAssetList(string assetList)
        {
            List<int> lstAuecAssets = new List<int>();

            object[] parameter = new object[1];
            parameter[0] = assetList;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAllCVAUECsForAssetList", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        lstAuecAssets.Add(Convert.ToInt32(row[0]));
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
            return lstAuecAssets;
        }


        /// <summary>
        /// This method gets the CV AUEC's present for a particular given list of assets and counter party venue.
        /// </summary>
        /// <param name="assetList"></param>
        /// /// <param name="cvID"></param>
        /// <returns>The collection of <see cref="AUEC"/> objects.</returns>
        public static AUECs GetAllCVAUECsForCVAndAssetList(string assetList, int cvID)
        {
            AUECs aUECs = new AUECs();

            object[] parameter = new object[2];
            parameter[0] = assetList;
            parameter[1] = cvID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAllCVAUECsForCVAndAssetList", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        aUECs.Add(FillCVAUECForThirdParty(row, 0));
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
            return aUECs;
        }

        #endregion

        /// <summary>
        /// This method fetches the aUECs corresponding to the particular counterPartyVenueID.
        /// </summary>
        /// <param name="counterPartyVenueID">The parameter against which the currencies are fetched.</param>
        /// <returns>Collecion of <see cref="AUEC"/> in <see cref="AUECs"/></returns>
        public static AUECs GetCVAUECs(int counterPartyVenueID)
        {
            AUECs aUECs = new AUECs();

            object[] parameter = new object[1];
            parameter[0] = counterPartyVenueID;

            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCVAUECs", parameter))
            {
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    aUECs.Add(FillCVAUEC(row, 0));
                }
            }
            return aUECs;
        }

        public static int SaveCVAUEC(int counterPartyVenueID)
        {
            int result = int.MinValue;
            object[] parameter = new object[1];

            try
            {
                parameter = new object[1];
                parameter[0] = counterPartyVenueID;
                result = DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveCVAUEC", parameter);
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

        public static CounterPartyVenue GetCVAUECDetails(int counterPartyVenueID, int auecID)
        {
            CounterPartyVenue counterPartyVenue = new CounterPartyVenue();

            object[] parameter = new object[2];
            parameter[0] = counterPartyVenueID;
            parameter[1] = auecID;

            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCVAUECDetails", parameter))
            {
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    counterPartyVenue = FillCVAUECDetails(row, 0);
                }
            }
            return counterPartyVenue;
        }

        /// <summary>
        /// Method to fetch the row or detail from the table against CVAUECID.
        /// </summary>
        /// <param name="cvAUECID">The primary parameter agaist which the details are fetched.</param>
        /// <returns>AUEC <see cref="AUEC"/> object. </returns>
        public static CounterPartyVenue GetCVAUEC(int cvAUECID)
        {
            CounterPartyVenue cvAUEC = new CounterPartyVenue();

            object[] parameter = new object[1];
            parameter[0] = cvAUECID;

            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCVAUECForCVAUECID", parameter))
            {
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    cvAUEC = FillCVAUECDetails(row, 0);
                }
            }
            return cvAUEC;
        }

        #endregion

        #region CVAUECCompliance
        public static CounterPartyVenue FillCVAUECCompliance(object[] row, int offSet)
        {
            int cvAUECComplianceID = 0 + offSet;
            int cvAUECID = 1 + offSet;
            int followCompliance = 2 + offSet;
            int shortSellConfirmation = 3 + offSet;
            int identifierID = 4 + offSet;
            int foreignID = 5 + offSet;

            CounterPartyVenue counterPartyVenue = new CounterPartyVenue();
            try
            {

                if (!(row[cvAUECComplianceID] is System.DBNull))
                {
                    counterPartyVenue.CVAUECComplianceID = int.Parse(row[cvAUECComplianceID].ToString());
                }
                if (!(row[cvAUECID] is System.DBNull))
                {
                    counterPartyVenue.CVAUECID = int.Parse(row[cvAUECID].ToString());
                }
                if (!(row[followCompliance] is System.DBNull))
                {
                    counterPartyVenue.FollowCompliance = int.Parse(row[followCompliance].ToString());
                }
                if (!(row[shortSellConfirmation] is System.DBNull))
                {
                    counterPartyVenue.ShortSellConfirmation = int.Parse(row[shortSellConfirmation].ToString());
                }
                if (!(row[identifierID] is System.DBNull))
                {
                    counterPartyVenue.IdentifierID = int.Parse(row[identifierID].ToString());
                }
                if (!(row[foreignID] is System.DBNull))
                {
                    counterPartyVenue.ForeignID = row[foreignID].ToString();
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
            return counterPartyVenue;
        }

        public static CounterPartyVenue GetCVAUECCompliance(int cvAUECID)
        {
            CounterPartyVenue counterPartyVenue = new CounterPartyVenue();

            object[] parameter = new object[1];
            parameter[0] = cvAUECID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCVAUECCompliance", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        counterPartyVenue = FillCVAUECCompliance(row, 0);
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
            return counterPartyVenue;
        }

        public static int SaveCVAUECCompliance(CounterPartyVenue counterPartyVenue)
        {
            int result = int.MinValue;

            object[] parameter = new object[6];

            parameter[0] = counterPartyVenue.CVAUECID;
            parameter[1] = counterPartyVenue.FollowCompliance;
            parameter[2] = counterPartyVenue.ShortSellConfirmation;
            parameter[3] = counterPartyVenue.IdentifierID;
            parameter[4] = counterPartyVenue.ForeignID;
            parameter[5] = int.MinValue;

            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCVAUECCompliance", parameter).ToString());
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

        #region CVCurrency
        public static Currency FillCVCurrencies(object[] row, int offSet)
        {
            int COUNTERPARTYVENUEID = 0 + offSet;
            int CURRENCYID = 1 + offSet;

            Currency currency = new Currency();
            try
            {
                if (row[COUNTERPARTYVENUEID] != null)
                {
                    currency.CounterPartyVenueID = int.Parse(row[COUNTERPARTYVENUEID].ToString());
                }
                if (row[CURRENCYID] != null)
                {
                    currency.CurencyID = int.Parse(row[CURRENCYID].ToString());
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
            return currency;
        }

        /// <summary>
        /// This method fetches the currencies corresponding to the particular counterPartyVenueID.
        /// </summary>
        /// <param name="counterPartyVenueID">The parameter against which the currencies are fetched.</param>
        /// <returns>Collecion of <see cref="Currency"/> in <see cref="Currency"/></returns>
        public static Currencies GetCVCurrencies(int counterPartyVenueID)
        {
            Currencies currencies = new Currencies();

            object[] parameter = new object[1];
            parameter[0] = counterPartyVenueID;

            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCVCurrencies", parameter))
            {
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    currencies.Add(FillCVCurrencies(row, 0));
                }
            }
            return currencies;
        }

        public static int SaveCVCurrency(Currencies currencies, int counterPartyVenueID)
        {
            int result = int.MinValue;

            try
            {
                object[] parameter = new object[1];
                parameter[0] = counterPartyVenueID;
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCVCurrencies", parameter).ToString();
                foreach (Currency currency in currencies)
                {
                    parameter = new object[3];
                    parameter[0] = counterPartyVenueID;
                    parameter[1] = currency.CurencyID;
                    parameter[2] = int.MinValue;
                    result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCVCurrencies", parameter).ToString());
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


        #region Symbol Mapping

        private static SymbolMapping FillCVSymbolMappings(object[] row, int offSet)
        {
            int mappedSymbolID = 0 + offSet;
            int cvAuecID = 1 + offSet;
            int symbol = 2 + offSet;
            int mappedSymbol = 3 + offSet;
            int counterPartyVenueID = 4 + offSet;

            SymbolMapping cvSymbolMapping = new SymbolMapping();
            try
            {
                if (row[mappedSymbolID] != null)
                {
                    cvSymbolMapping.CVSymboMappingID = int.Parse(row[mappedSymbolID].ToString());
                }
                if (row[cvAuecID] != null)
                {
                    cvSymbolMapping.CVAUECID = int.Parse(row[cvAuecID].ToString());
                }
                if (row[symbol] != null)
                {
                    cvSymbolMapping.Symbol = row[symbol].ToString();
                }
                if (row[mappedSymbol] != null)
                {
                    cvSymbolMapping.MappedSymbol = row[mappedSymbol].ToString();
                }
                if (row[counterPartyVenueID] != null)
                {
                    cvSymbolMapping.CounterPartyVenueID = int.Parse(row[counterPartyVenueID].ToString());
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
            return cvSymbolMapping;
        }

        public static SymbolMappings GetCVSymbolMappings()
        {
            SymbolMappings cvSymbolMappings = new SymbolMappings();

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllSymbolMappings";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        cvSymbolMappings.Add(FillCVSymbolMappings(row, 0));
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
            return cvSymbolMappings;
        }

        public static SymbolMappings GetCVSymbolMapping(int counterPartyVenueID)
        {
            object[] parameter = new object[1];
            parameter[0] = counterPartyVenueID;
            SymbolMappings cvSymbolMappings = new SymbolMappings();

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetSymbolMapping", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        cvSymbolMappings.Add(FillCVSymbolMappings(row, 0));
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
            return cvSymbolMappings;
        }

        public static bool DeleteCVSymbolMapping(int counterPartyVenueID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            parameter[0] = counterPartyVenueID;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteSymbolMapping", parameter) > 0)
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
            return result;
        }

        public static bool DeleteCVSymbolMappingByID(int cvSymbolMappingID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            parameter[0] = cvSymbolMappingID;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteSymbolMappingByID", parameter) > 0)
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
            return result;
        }

        public static int SaveCVSymbolMapping(SymbolMappings cvSymbolMappings)
        {
            int result = int.MinValue;

            object[] parameter = new object[4];
            try
            {
                foreach (SymbolMapping cvSymbolMapping in cvSymbolMappings)
                {
                    parameter[0] = cvSymbolMapping.CVSymboMappingID;
                    parameter[1] = cvSymbolMapping.CVAUECID;
                    parameter[2] = cvSymbolMapping.Symbol;
                    parameter[3] = cvSymbolMapping.MappedSymbol;

                    result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveSymbolMapping", parameter).ToString());
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

        #region CVFIX
        public static CounterPartyVenue FillCVFIX(object[] row, int offSet)
        {
            int cvFixID = 0 + offSet;
            int counterPartyVenueID = 1 + offSet;
            int acronymn = 2 + offSet;
            int fixVersionID = 3 + offSet;
            int targetCompID = 4 + offSet;
            int deliverToCompID = 5 + offSet;
            int deliverToSubID = 6 + offSet;

            CounterPartyVenue counterPartyVenue = new CounterPartyVenue();
            try
            {

                if (!(row[cvFixID] is System.DBNull))
                {
                    counterPartyVenue.CVFIXID = int.Parse(row[cvFixID].ToString());
                }
                if (!(row[counterPartyVenueID] is System.DBNull))
                {
                    counterPartyVenue.CounterPartyVenueID = int.Parse(row[counterPartyVenueID].ToString());
                }
                if (!(row[acronymn] is System.DBNull))
                {
                    counterPartyVenue.Acronym = row[acronymn].ToString();
                }
                if (!(row[fixVersionID] is System.DBNull))
                {
                    counterPartyVenue.FixVersionID = int.Parse(row[fixVersionID].ToString());
                }
                if (!(row[targetCompID] is System.DBNull))
                {
                    counterPartyVenue.TargetCompID = row[targetCompID].ToString();
                }
                if (!(row[deliverToCompID] is System.DBNull))
                {
                    counterPartyVenue.DeliverToCompID = row[deliverToCompID].ToString();
                }
                if (!(row[deliverToSubID] is System.DBNull))
                {
                    counterPartyVenue.DeliverToSubID = row[deliverToSubID].ToString();
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
            return counterPartyVenue;
        }

        public static CounterPartyVenue GetCVFIX(int counterPartyVenueID)
        {
            CounterPartyVenue counterPartyVenue = new CounterPartyVenue();

            object[] parameter = new object[1];
            parameter[0] = counterPartyVenueID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCVFIX", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        counterPartyVenue = FillCVFIX(row, 0);
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
            return counterPartyVenue;
        }

        #region CVFix Details For CompanyCounterPartyVenueID
        public static CounterPartyVenue GetCVFIXForCompanyCPVID(int companyCounterPartyVenueID)
        {
            CounterPartyVenue counterPartyVenue = new CounterPartyVenue();

            object[] parameter = new object[1];
            parameter[0] = companyCounterPartyVenueID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCVFIXForCompanyCPVID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        counterPartyVenue = FillCVFIX(row, 0);
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
            return counterPartyVenue;
        }
        #endregion

        public static int SaveCVFIX(CounterPartyVenue counterPartyVenue)
        {
            int result = int.MinValue;

            object[] parameter = new object[7];

            parameter[0] = counterPartyVenue.CounterPartyVenueID;
            parameter[1] = counterPartyVenue.Acronym;
            parameter[2] = counterPartyVenue.FixVersionID;
            parameter[3] = counterPartyVenue.TargetCompID;
            parameter[4] = counterPartyVenue.DeliverToCompID;
            parameter[5] = counterPartyVenue.DeliverToSubID;
            parameter[6] = int.MinValue;

            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCVFIX", parameter).ToString());
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
        public static bool DeleteCVFIX(int counterPartyVenueID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            parameter[0] = counterPartyVenueID;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCVFIX", parameter) > 0)
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
            return result;
        }
        #endregion


        /// <summary>
        /// Get Counter Party Account Mapping From Db
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        internal static Dictionary<int, List<int>> GetCounterPartyAccountMappingFromDb(int companyID)
        {
            Dictionary<int, List<int>> counterPartyAccountMapping = new Dictionary<int, List<int>>();
            try
            {
                object[] param = { companyID };
                string sProc = "P_GetCounterPartyAccountMapping";
                using (IDataReader dr = DatabaseManager.DatabaseManager.ExecuteReader(sProc, param))
                {
                    while (dr.Read())
                    {
                        int counterPartyId = Convert.ToInt32(dr[0]);
                        int companyAccountId = Convert.ToInt32(dr[1]);
                        if (counterPartyAccountMapping.ContainsKey(counterPartyId))
                            counterPartyAccountMapping[counterPartyId].Add(companyAccountId);
                        else
                        {
                            List<int> counterPartyAccount = new List<int>();
                            counterPartyAccount.Add(companyAccountId);
                            counterPartyAccountMapping.Add(counterPartyId, counterPartyAccount);
                        }
                    }
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
            return counterPartyAccountMapping;
        }

        /// <summary>
        /// Get Fund wise Executing Broker Mapping From DB
        /// </summary>
        /// <param name="companyID"></param>
        internal static Dictionary<int,int> GetFundWiseExecutingBrokerMappingFromDB(int companyID)
        {
            Dictionary<int, int> fundWiseExecutingBrokerMapping = new Dictionary<int, int>();
            try
            {
                object[] param = { companyID };
                string sProc = "P_GetFundWiseExecutingBroker";
                using (IDataReader dr = DatabaseManager.DatabaseManager.ExecuteReader(sProc, param))
                {
                    while (dr.Read())
                    {
                        int fundId = Convert.ToInt32(dr[0]);
                        int brokerId = Convert.ToInt32(dr[1]);
                        fundWiseExecutingBrokerMapping.Add(fundId, brokerId);                      
                    }
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
            return fundWiseExecutingBrokerMapping;
        }

 		/// <summary>
        /// Save Fund wise Executing Broker Mapping in DB
        /// </summary>
        internal static void SaveFundWiseExecutingBroker(string xmlDocExecutingBrokerMapping, int companyID)
        {
            try
            {
                object[] parameter = { xmlDocExecutingBrokerMapping, companyID };
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveFundWiseExecutingBroker", parameter);             
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

        internal static bool GetBreakOrderPrefernceFromDB(int companyID)
        {
            object[] parameters = new object[1];
            parameters[0] = companyID;
            bool isBreakOrderPreference = false;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetBreakOrderPreferenceFromDB", parameters))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row != null)
                        {
                            if (row[0] != System.DBNull.Value)
                            {
                                isBreakOrderPreference = Convert.ToBoolean(row[0]);
                            }
                        }
                    }
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
            return isBreakOrderPreference;
        }
        /// <summary>
        /// Get Counter Party Auec Mapping From Db
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        internal static Dictionary<int, List<int>> GetCounterPartyAuecMappingFromDb(int companyID)
        {
            Dictionary<int, List<int>> counterPartyAuecMapping = new Dictionary<int, List<int>>();
            try
            {
                object[] param = { companyID };
                string sProc = "P_GetCounterPartyAUECMapping";
                using (IDataReader dr = DatabaseManager.DatabaseManager.ExecuteReader(sProc, param))
                {
                    while (dr.Read())
                    {
                        int counterPartyId = Convert.ToInt32(dr[0]);
                        int companyAUECId = Convert.ToInt32(dr[1]);
                        if (counterPartyAuecMapping.ContainsKey(counterPartyId))
                            counterPartyAuecMapping[counterPartyId].Add(companyAUECId);
                        else
                        {
                            List<int> counterPartyAUEC = new List<int>();
                            counterPartyAUEC.Add(companyAUECId);
                            counterPartyAuecMapping.Add(counterPartyId, counterPartyAUEC);
                        }
                    }
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
            return counterPartyAuecMapping;
        }


        internal static int SaveCounterPartyAUECMapping(string xmlDocCVAUECMapping, int companyID)
        {
            int i = 0;
            try
            {
                object[] parameter = { xmlDocCVAUECMapping, "CouldNotSaveMapping", -1, companyID };

                //Modified by Faisal Shah 18/07/14
                object result = DatabaseManager.DatabaseManager.ExecuteScalarWithTimeOut("P_SaveCounterPartyAUECMapping", parameter);
                if (result != null)
                    i = result == null ? 0 : (int)result;
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
            return i;
        }

        internal static void SaveBreakOrderPreference(int companyID, bool valueToBeSaved)
        {
            object[] parameter = { companyID, valueToBeSaved };
            try
            {
                DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveBreakOrderPreference", parameter);
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
        internal static int SaveCounterPartyAccountMapping(string xmlDocCVAccountMapping, int companyID)
        {

            int i = 0;
            try
            {
                object[] parameter = { xmlDocCVAccountMapping, "CouldNotSaveMapping", -1, companyID };

                //Modified by Faisal Shah 18/07/14
                object result = DatabaseManager.DatabaseManager.ExecuteScalarWithTimeOut("P_SaveCounterPartyAccountMapping", parameter);
                if (result != null)
                    i = result == null ? 0 : (int)result;
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
            return i;
        }
    }
}
