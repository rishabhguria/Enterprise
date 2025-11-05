using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.Admin.BLL
{
    internal class ReleaseSetupDAL
    {
        /// <summary>
        /// Get the clients from database
        /// </summary>
        /// <returns>The dictionary holding the clients ID and the clients name</returns>
        internal static Dictionary<int, string> GetClientsFromDB()
        {
            Dictionary<int, string> dicClient = new Dictionary<int, string>();

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetCompanies";

                using (IDataReader drClient = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (drClient.Read())
                    {
                        dicClient.Add(drClient.GetInt32(0), drClient.GetString(1));
                    }
                }
                return dicClient;
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
            return null;
        }

        /// <summary>
        /// Get the List of accounts from DB
        /// </summary>
        /// <returns>Dictionary holding the IDs and names of the accounts</returns>
        internal static Dictionary<int, string> GetAccountsFromDB(String xmlClient)
        {
            Dictionary<int, string> dicAccounts = new Dictionary<int, string>();
            try
            {
                object[] param = { xmlClient };
                using (IDataReader drAccounts = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAllCompanywiseFunds", param))
                {
                    while (drAccounts.Read())
                    {
                        dicAccounts.Add(drAccounts.GetInt32(0), drAccounts.GetString(1));
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
            return dicAccounts;
        }

        /// <summary>
        /// Get the details of the release from database
        /// </summary>
        /// <returns>the release details dictionary</returns>
        internal static Dictionary<int, ReleaseDetails> GetReleaseDetailsFromDB()
        {
            Dictionary<int, ReleaseDetails> dicRelease = new Dictionary<int, ReleaseDetails>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllReleaseDetails";

                using (IDataReader drRelease = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (drRelease.Read())
                    {
                        int releaseID = drRelease.GetInt32(0);
                        if (dicRelease.ContainsKey(releaseID))
                        {
                            if (!dicRelease[releaseID].clientID.Contains(drRelease.GetInt32(1)))
                            {
                                dicRelease[releaseID].clientID.Add(drRelease.GetInt32(1));
                            }
                            if (!dicRelease[releaseID].accountID.Contains(drRelease.GetInt32(2)))
                            {
                                dicRelease[releaseID].accountID.Add(drRelease.GetInt32(2));
                            }
                            dicRelease[releaseID].ReleaseName = drRelease.GetString(3);
                            dicRelease[releaseID].IP = drRelease.GetString(4);
                            dicRelease[releaseID].ReleasePath = drRelease.GetString(5);
                            dicRelease[releaseID].ClientDB_Name = drRelease.GetString(6);
                            dicRelease[releaseID].SMDB_Name = drRelease.GetString(7);
                            dicRelease[releaseID].InUse = drRelease.GetInt32(8);
                        }
                        else
                        {
                            ReleaseDetails pRelease = new ReleaseDetails();
                            pRelease.clientID.Add(drRelease.GetInt32(1));
                            pRelease.accountID.Add(drRelease.GetInt32(2));
                            pRelease.ReleaseName = drRelease.GetString(3);
                            pRelease.IP = drRelease.GetString(4);
                            pRelease.ReleasePath = drRelease.GetString(5);
                            pRelease.ClientDB_Name = drRelease.GetString(6);
                            pRelease.SMDB_Name = drRelease.GetString(7);
                            pRelease.InUse = drRelease.GetInt32(8);
                            dicRelease.Add(drRelease.GetInt32(0), pRelease);
                        }
                    }
                }
                return dicRelease;
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
            return null;
        }

        /// <summary>
        /// Save the release details in the database
        /// </summary>
        /// <param name="xmlReleaseDetails">XML representation of the details</param>
        /// <param name="companyID">ID of the company</param>
        public static void SaveReleaseDetailsInDB(string xmlReleaseDetails)
        {
            object[] parameter = { xmlReleaseDetails };

            string sProcSave = "P_SaveReleaseDetails";
            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery(sProcSave, parameter);
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
    }
}
