using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Data;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// VenueManager would be a static class to handel <see cref="Venue"/> related details.
    /// </summary>
    /// <remarks>Its a Static class like other managers class so that we don't have to instatiate it again and again while working with them.</remarks>
    public class VenueManager
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private VenueManager()
        {
        }

        #endregion

        /// <summary>
        /// Adds/Updates <see cref="Venue"/> into database.
        /// </summary>
        /// <param name="venue"><see cref="Venue"/> to be added.</param>
        /// <returns>Result. True if succesfull else False.</returns>
        //#region Basic methods like Add/Update/Delete/Get

        public static Venue FillVenues(object[] row, int offSet)
        {
            int venueID = 0 + offSet;
            int venueName = 1 + offSet;
            int venuetype = 2 + offSet;
            int route = 3 + offSet;
            int exchangeID = 4 + offSet;

            Venue venue = new Venue();
            try
            {
                if (row[venueID] != System.DBNull.Value)
                {
                    venue.VenueID = int.Parse(row[venueID].ToString());
                }
                if (row[venueName] != System.DBNull.Value)
                {
                    venue.VenueName = row[venueName].ToString();
                }
                if (row[venuetype] != System.DBNull.Value)
                {
                    venue.VenueTypeID = int.Parse(row[venuetype].ToString());
                }
                if (row[route] != System.DBNull.Value)
                {
                    venue.Route = row[route].ToString();
                }
                if (row[exchangeID] != System.DBNull.Value)
                {
                    venue.ExchangeID = int.Parse(row[exchangeID].ToString());
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
            return venue;
        }

        /// <summary>
        /// Gets all <see cref="Venue"/> from datatbase.
        /// </summary>
        /// <returns><see cref="Venue"/> fetched.</returns>
        public static Venues GetVenues()
        {
            Venues venues = new Venues();

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllVenues";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        venues.Add(FillVenues(row, 0));
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
            return venues;
        }

        /// <summary>
        /// Gets <see cref="Venue"/> from the database.
        /// </summary>
        /// <param name="venueID">ID of <see cref="Venue"/> to be fetched from database.</param>
        /// <returns><see cref="Venue"/> fetched.</returns>
        public static Venue GetVenue(int venueID)
        {
            Venue venue = new Venue();

            object[] parameter = new object[1];
            parameter[0] = venueID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetVenue", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        venue = FillVenues(row, 0);
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
            return venue;
        }

        /// <summary>
        /// Gets <see cref="Venue"/> from the database.
        /// </summary>
        /// <param name="exchangeID">ID of <see cref="Venue"/> to be fetched from database.</param>
        /// <returns><see cref="Venue"/> fetched.</returns>
        public static Venue GetVenueByExchangeID(int exchangeID)
        {
            Venue venue = new Venue();

            object[] parameter = new object[1];
            parameter[0] = exchangeID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetVenueByExchangeID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        venue = FillVenues(row, 0);
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
            return venue;
        }

        public static Venues GetCounterPartyVenues(int counterPartyID)
        {
            Venues venues = new Venues();
            object[] parameter = new object[1];
            parameter[0] = counterPartyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCounterPartyVenuesForCounterParty", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        venues.Add(FillVenues(row, 0));
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
            return venues;
        }

        public static Venues GetCounterPartyVenueNames(int counterPartyID)
        {
            Venues venues = new Venues();
            object[] parameter = new object[1];
            parameter[0] = counterPartyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCounterPartyVenueNames", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        venues.Add(FillVenues(row, 0));
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
            return venues;
        }


        /// <summary>
        /// Deletes <see cref="Venue"/> from the database.
        /// </summary>
        /// <param name="venueID">ID of the <see cref="Venue"/> to be deleted.</param>
        /// <returns>Result. True if succesfull else False.</returns>
        public static bool DeleteVenue(int venueID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            parameter[0] = venueID;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteVenue", parameter) > 0)
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
        /// Adds/Updates <see cref="Venue"/> into database.
        /// </summary>
        /// <param name="venue"><see cref="Venue"/> to be added.</param>
        /// <returns>Result. True if succesfull else False.</returns>

        public static int SaveVenue(Venue venue)
        {
            int result = int.MinValue;

            object[] parameter = new object[6];

            parameter[0] = venue.VenueID;
            parameter[1] = venue.VenueName;
            parameter[2] = venue.VenueTypeID;
            parameter[3] = venue.Route;
            parameter[4] = venue.ExchangeID;
            parameter[5] = int.MinValue;
            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveVenue", parameter).ToString());
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

        //#endregion

        #region VenueTypes

        public static VenueType FillVenueTypes(object[] row, int offSet)
        {
            int venueTypeID = 0 + offSet;
            int venuetype = 1 + offSet;

            VenueType venueType = new VenueType();
            try
            {
                if (row[venueTypeID] != null)
                {
                    venueType.VenueTypeID = int.Parse(row[venueTypeID].ToString());
                }
                if (row[venuetype] != null)
                {
                    venueType.Type = row[venuetype].ToString();
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
            return venueType;
        }

        /// <summary>
        /// Gets all <see cref="VenueType"/> from datatbase.
        /// </summary>
        /// <returns><see cref="VenueType"/> fetched.</returns>
        public static VenueTypes GetVenueTypes()
        {
            VenueTypes venueTypes = new VenueTypes();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllVenueTypes";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        venueTypes.Add(FillVenueTypes(row, 0));
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
            return venueTypes;
        }

        public static int SaveVenueType(VenueType venueType)
        {
            int result = int.MinValue;

            object[] parameter = new object[3];

            parameter[0] = venueType.VenueTypeID;
            parameter[1] = venueType.Type;
            parameter[2] = int.MinValue;
            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveVenueType", parameter).ToString());
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

        public static bool DeleteVenueType(int venueTypeID)
        {
            bool result = false;
            try
            {
                object[] parameter = new object[1];
                parameter[0] = venueTypeID;
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteVenueType", parameter) > 0)
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
    }
}
