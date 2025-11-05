#region using

using Prana.LogManager;
using System;
using System.Data;

#endregion

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for RiskClientManager.
    /// </summary>
    public class RiskClientManager
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>

        public RiskClientManager()
        {

        }
        #endregion

        #region Basic methods like Save/Get/Fill/Delete for ClientOverallLimit

        //fillClientOverallLimitDetails

        /// <summary>
        /// the method is used to fill an Object of class ClientOverallLimits
        /// </summary>
        /// <param name="row"></param>
        /// <param name="offSet"></param>
        /// <returns></returns>
        public static ClientOverallLimit FillClientOverallLimit(object[] row, int offSet)
        {
            int companyClientRMID = 0 + offSet;
            int clientID = 1 + offSet;
            int clientExposureLimit = 2 + offSet;
            int companyID = 3 + offSet;
            int clientName = 4 + offSet;

            ClientOverallLimit clientOverallLimit = new ClientOverallLimit();
            try
            {
                if (row[companyClientRMID] != null)
                {
                    clientOverallLimit.CompanyClientRMID = int.Parse(row[companyClientRMID].ToString());
                }
                if (row[clientID] != null)
                {
                    clientOverallLimit.ClientID = int.Parse(row[clientID].ToString());
                }
                if (row[clientExposureLimit] != null)
                {
                    clientOverallLimit.ClientExposureLimit = int.Parse(row[clientExposureLimit].ToString());
                }
                if (row[companyID] != null)
                {
                    clientOverallLimit.CompanyID = int.Parse(row[companyID].ToString());
                }
                if (row[clientName] != null)
                {
                    clientOverallLimit.ClientName = row[clientName].ToString();
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
            return clientOverallLimit;
        }

        //save method

        /// <summary>
        /// the method is used to save a ClientOverallLimit Object.
        /// </summary>
        /// <param name="clientOverallLimit"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static int SaveClientOverallLimit(ClientOverallLimit clientOverallLimit, int companyID)
        {
            int result = int.MinValue;

            try
            {
                object[] parameter = new object[5];

                parameter[0] = clientOverallLimit.CompanyClientRMID;
                parameter[1] = clientOverallLimit.ClientID;
                parameter[2] = clientOverallLimit.ClientExposureLimit;
                parameter[3] = companyID;
                parameter[4] = int.MinValue;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveClientOverallDetail", parameter).ToString());

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

        //Get ClientOverallLimits Collection

        /// <summary>
        /// fetching all the existing ClientOverallLimits data to bind in grid
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static ClientOverallLimits GetAllClientOverallLimits(int companyID)
        {
            ClientOverallLimits clientOverallLimits = new ClientOverallLimits();

            Object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAllClientOverallLimits", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        Prana.Admin.BLL.ClientOverallLimit _dshfsdh = FillClientOverallLimit(row, 0);
                        clientOverallLimits.Add(_dshfsdh);
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
            return clientOverallLimits;
        }

        // Get a ClientOverallLimit Object

        /// <summary>
        /// The method id used to fetch a single ClientOverallLimit Object 
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public static ClientOverallLimit GetClientOverallLimit(int companyID, int clientID)
        {
            ClientOverallLimit clientOverallLimit = new ClientOverallLimit();

            object[] parameter = new object[2];
            parameter[0] = companyID;
            parameter[1] = clientID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetClientOverallbyCompanyClientID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        clientOverallLimit = FillClientOverallLimit(row, 0);
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
            return clientOverallLimit;
        }

        //Delete ClientOverallLimit

        /// <summary>
        /// The method is used to delete a ClientOverallLimit's details.
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="companyClientID"></param>
        /// <returns></returns>
        public static bool DeleteClientOverallLimit(int companyID, int companyClientID)
        {
            bool result = false;
            Object[] parameter = new object[2];
            parameter[0] = companyID;
            parameter[1] = companyClientID;

            try
            {
                if ((DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteClientOverallLimit", parameter)) > 0)
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            #endregion
            return result;
        }

        //Delete ClientOverallLimits

        /// <summary>
        /// The method is used to delete all ClientOverallLimits for a company.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static bool DeleteAllClientOverallLimits(int companyID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteAllClientOverallLimits", parameter) > 0)
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
